using HCAS.Database.AppDbContextModels;
using HCAS.Domain.Features.Doctors;
using HCAS.Domain.Features.Appointment.Models;
using HCAS.Shared;
using Microsoft.EntityFrameworkCore;

namespace HCAS.Domain.Features.Appointment;

public static class AppointmentQuery
{
    public const string GetAll = @"
            SELECT 
                a.Id, 
                a.DoctorId,
                d.Name AS DoctorName, 
                a.PatientId,
                p.Name AS PatientName, 
                a.ScheduleId,
                a.AppointmentDate, 
                a.AppointmentNumber,
                a.Status
            FROM Appointments a
            INNER JOIN Doctors d ON a.DoctorId = d.Id
            INNER JOIN Patients p ON a.PatientId = p.Id";

    public const string GetById = @"
            SELECT 
                a.Id, 
                a.DoctorId,
                d.Name AS DoctorName, 
                a.PatientId,
                p.Name AS PatientName, 
                a.ScheduleId,
                a.AppointmentDate, 
                a.AppointmentNumber,
                a.Status
            FROM Appointments a
            INNER JOIN Doctors d ON a.DoctorId = d.Id
            INNER JOIN Patients p ON a.PatientId = p.Id
            WHERE a.Id = @Id";

    public const string CountBySchedule = @"
            SELECT COUNT(*) 
            FROM Appointments 
            WHERE ScheduleId = @ScheduleId AND Status <> 'Cancelled'";

    public const string Insert = @"
            INSERT INTO Appointments 
                (DoctorId, PatientId, ScheduleId, AppointmentDate, AppointmentNumber, Status, del_flg) 
            OUTPUT INSERTED.Id
            VALUES (@DoctorId, @PatientId, @ScheduleId, @AppointmentDate, @AppointmentNumber, @Status, @DelFlag)";

    public const string UpdateStatus = @"
            UPDATE Appointments 
            SET Status = @NewStatus 
            WHERE Id = @AppointmentId";

    public const string Delete = @"
            DELETE FROM Appointments WHERE Id = @AppointmentId";
}

public class AppointmentService
{
    private readonly DapperService _dapper;
    private readonly AppDbContext _appDbContext;

    public AppointmentService(DapperService dapperService, AppDbContext appDbContext)
    {
        _dapper = dapperService;
        _appDbContext = appDbContext;
    }

    public async Task<Result<IEnumerable<AppointmentResModel>>> GetAllAppointments()
    {
        try
        {
            var result = await _dapper.QueryAsync<AppointmentResModel>(AppointmentQuery.GetAll);
            var message = result.Any() ? "Success" : "No appointments found";
            return Result<IEnumerable<AppointmentResModel>>.Success(result, message);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<AppointmentResModel>>.SystemError(
                $"Error retrieving appointments: {ex.Message}");
        }
    }

    public async Task<Result<PagedResult<AppointmentResponseModel>>> GetAppointmentsAsync(
        int page = 1, int pageSize = 10, 
        string? doctorName = null, string? patientName = null,
        bool includePastAppointments = false)
    {
        try
        {
            var today = DateTime.Today;
            var query = _appDbContext.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Schedule)
                .AsQueryable();

            // Filter by date: show only current date and future appointments by default
            // If includePastAppointments is true, show only past appointments
            if (includePastAppointments)
            {
                // Show only past appointments (before today)
                query = query.Where(a => a.AppointmentDate.Date < today);
            }
            else
            {
                // Show only current date and future appointments (from today onwards)
                query = query.Where(a => a.AppointmentDate.Date >= today);
            }

            if (!string.IsNullOrWhiteSpace(doctorName))
            {
                query = query.Where(a =>
                    a.Doctor.Name.Contains(doctorName));
            }
            
            if (!string.IsNullOrWhiteSpace(patientName))
            {
                query = query.Where(a =>
                    a.Patient.Name.Contains(patientName));
            }

            var total = await query.CountAsync();

            var appointments = await query
                .OrderBy(a => a.Doctor.Name)  // First order by doctor name (ascending)
                .ThenBy(a => a.Schedule != null && a.Schedule.ScheduleDate.HasValue 
                    ? a.Schedule.ScheduleDate.Value 
                    : a.AppointmentDate.Date)  // Then by schedule date (ascending)
                .ThenBy(a => a.AppointmentDate)  // Then by appointment date/time (ascending)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AppointmentResponseModel
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    PatientId = a.PatientId,
                    ScheduleId = a.ScheduleId,
                    DoctorName = a.Doctor.Name,
                    PatientName = a.Patient.Name,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentNumber = a.AppointmentNumber,
                    Status = a.Status
                })
                .ToListAsync();

            var pagedResult = new PagedResult<AppointmentResponseModel>
            {
                Items = appointments,
                TotalCount = total
            };

            return appointments.Any()
                ? Result<PagedResult<AppointmentResponseModel>>.Success(pagedResult)
                : Result<PagedResult<AppointmentResponseModel>>.NotFound("No appointments found");
        }
        catch (Exception ex)
        {
            return Result<PagedResult<AppointmentResponseModel>>.SystemError(ex.Message);
        }
    }
    
    public async Task<Result<AppointmentResModel>> GetAppointmentById(int id)
    {
        try
        {
            var appointment =
                await _dapper.QueryFirstOrDefaultAsync<AppointmentResModel>(AppointmentQuery.GetById,
                    new { Id = id });
            var message = appointment == null ? "No appointment found" : "Success";
            return Result<AppointmentResModel>.Success(appointment, message);
        }
        catch (Exception ex)
        {
            return Result<AppointmentResModel>.SystemError($"Error retrieving appointment: {ex.Message}");
        }
    }

    private const int AppointmentDurationMinutes = 20;

    public async Task<Result<AppointmentResModel>> CreateAppointment(int patientId, int scheduleId)
    {
        try
        {
            if (patientId <= 0)
                return Result<AppointmentResModel>.ValidationError("Patient doesn't exist.");

            var schedule = await _appDbContext.DoctorSchedules
                .FirstOrDefaultAsync(s => s.Id == scheduleId);

            if (schedule == null || !schedule.ScheduleDate.HasValue)
                return Result<AppointmentResModel>.ValidationError("Invalid schedule");

            // Get all existing appointments for this schedule (excluding cancelled)
            var existingAppointments = await _appDbContext.Appointments
                .Where(a => a.ScheduleId == scheduleId && a.Status != "Cancelled" && !a.DelFlg)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

            // Check if schedule is full based on max patients
            if (existingAppointments.Count >= schedule.MaxPatients)
                return Result<AppointmentResModel>.ValidationError("This schedule is already full");

            // Calculate the next available time slot (20 minutes apart)
            DateTime scheduleStartTime = schedule.ScheduleDate.Value;
            DateTime? nextAvailableTime = null;
            
            // If no existing appointments, use the schedule start time
            if (!existingAppointments.Any())
            {
                nextAvailableTime = scheduleStartTime;
            }
            else
            {
                // Find the next available 20-minute slot after the last appointment
                var lastAppointmentTime = existingAppointments.Last().AppointmentDate;
                var nextSlot = lastAppointmentTime.AddMinutes(AppointmentDurationMinutes);
                
                // Ensure we don't exceed the schedule's maximum time capacity
                // Assuming schedule duration based on MaxPatients * 20 minutes
                var maxScheduleEndTime = scheduleStartTime.AddMinutes(schedule.MaxPatients.Value * AppointmentDurationMinutes);
                
                if (nextSlot > maxScheduleEndTime)
                    return Result<AppointmentResModel>.ValidationError("No available time slots for this schedule");
                
                nextAvailableTime = nextSlot;
            }

            // Validate the calculated time is not in the past
            if (nextAvailableTime.Value < DateTime.Now)
                return Result<AppointmentResModel>.ValidationError("Calculated appointment time is in the past");

            int appointmentNumber = existingAppointments.Count + 1;

            var parameters = new
            {
                DoctorId = schedule.DoctorId,
                PatientId = patientId,
                ScheduleId = scheduleId,
                AppointmentDate = nextAvailableTime.Value,
                AppointmentNumber = appointmentNumber,
                Status = "Pending",
                DelFlag = false
            };

            var newId = await _dapper.QueryFirstOrDefaultAsync<int>(AppointmentQuery.Insert, parameters);

            var result = new AppointmentResModel
            {
                Id = newId,
                DoctorId = schedule.DoctorId,
                PatientId = patientId,
                ScheduleId = scheduleId,
                AppointmentDate = nextAvailableTime.Value,
                AppointmentNumber = appointmentNumber,
                Status = "Pending",
                DelFlg = false
            };

            return Result<AppointmentResModel>.Success(result, $"Appointment created successfully for {nextAvailableTime.Value:HH:mm}");
        }
        catch (Exception ex)
        {
            return Result<AppointmentResModel>.SystemError($"Error creating appointment: {ex.Message}");
        }
    }

    public async Task<Result<AppointmentResModel>> UpdateAppointment(int appointmentId, string newStatus)
    {
        try
        {
            if (appointmentId <= 0)
                return Result<AppointmentResModel>.ValidationError("Invalid AppointmentId");

            if (string.IsNullOrWhiteSpace(newStatus))
                return Result<AppointmentResModel>.ValidationError("Invalid status");

            var res = await _dapper.ExecuteAsync(AppointmentQuery.UpdateStatus,
                new { AppointmentId = appointmentId, NewStatus = newStatus });

            if (res != 1)
                return Result<AppointmentResModel>.SystemError("Failed to update appointment");

            var result = new AppointmentResModel
            {
                Id = appointmentId,
                Status = newStatus
            };

            return Result<AppointmentResModel>.Success(result, "Appointment updated successfully");
        }
        catch (Exception ex)
        {
            return Result<AppointmentResModel>.SystemError($"Error updating appointment: {ex.Message}");
        }
    }

    public async Task<Result<AppointmentResModel>> DeleteAppointment(int appointmentId)
    {
        try
        {
            var res = await _dapper.ExecuteAsync(AppointmentQuery.Delete, new { AppointmentId = appointmentId });

            if (res != 1)
                return Result<AppointmentResModel>.ValidationError("Failed to delete appointment");

            return Result<AppointmentResModel>.Success(new AppointmentResModel { Id = appointmentId },
                "Appointment deleted successfully");
        }
        catch (Exception ex)
        {
            return Result<AppointmentResModel>.SystemError($"Error deleting appointment: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets the next available appointment time for a schedule (20-minute slots)
    /// </summary>
    public async Task<DateTime?> GetNextAvailableAppointmentTime(int scheduleId)
    {
        try
        {
            var schedule = await _appDbContext.DoctorSchedules
                .FirstOrDefaultAsync(s => s.Id == scheduleId);

            if (schedule == null || !schedule.ScheduleDate.HasValue)
                return null;

            var existingAppointments = await _appDbContext.Appointments
                .Where(a => a.ScheduleId == scheduleId && a.Status != "Cancelled" && !a.DelFlg)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

            DateTime scheduleStartTime = schedule.ScheduleDate.Value;

            if (!existingAppointments.Any())
            {
                return scheduleStartTime;
            }

            var lastAppointmentTime = existingAppointments.Last().AppointmentDate;
            var nextSlot = lastAppointmentTime.AddMinutes(AppointmentDurationMinutes);

            var maxScheduleEndTime = scheduleStartTime.AddMinutes(schedule.MaxPatients.Value * AppointmentDurationMinutes);

            if (nextSlot > maxScheduleEndTime)
                return null;

            return nextSlot;
        }
        catch
        {
            return null;
        }
    }
}

public enum EnumAppointmentStatus
{
    Pending,
    Complete,
    Cancelled
}