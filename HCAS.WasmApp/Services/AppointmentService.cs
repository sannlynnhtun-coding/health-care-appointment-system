using HCAS.WasmApp.Models.Appointments;
using HCAS.WasmApp.Models.Doctors;
using HCAS.WasmApp.Models.Patients;
using HCAS.WasmApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCAS.WasmApp.Services;

public class AppointmentService
{
    private readonly IndexedDbService _dbService;
    private const string StoreName = "Appointments";

    public AppointmentService(IndexedDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<Result<List<AppointmentResModel>>> GetAppointmentsAsync()
    {
        try
        {
            var appointments = await _dbService.GetAllAsync<AppointmentResModel>(StoreName);
            var doctors = await _dbService.GetAllAsync<DoctorsResModel>("Doctors");
            var patients = await _dbService.GetAllAsync<PatientResModel>("Patients");

            var result = appointments.Where(a => !a.DelFlg).Select(a => {
                a.DoctorName = doctors.FirstOrDefault(d => d.Id == a.DoctorId)?.Name ?? "Unknown";
                a.PatientName = patients.FirstOrDefault(p => p.Id == a.PatientId)?.Name ?? "Unknown";
                return a;
            }).ToList();

            return Result<List<AppointmentResModel>>.Success(result);
        }
        catch (System.Exception ex)
        {
            return Result<List<AppointmentResModel>>.SystemError(ex.Message);
        }
    }

    public async Task<Result<AppointmentResModel>> CreateAppointmentAsync(AppointmentReqModel dto)
    {
        try
        {
            var all = await _dbService.GetAllAsync<AppointmentResModel>(StoreName);
            int nextId = all.Any() ? all.Max(a => a.Id) + 1 : 1;

            var appointment = new AppointmentResModel
            {
                Id = nextId,
                ScheduleId = dto.ScheduleId,
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                AppointmentDate = dto.AppointmentDate,
                AppointmentNumber = dto.AppointmentNumber,
                Status = dto.Status,
                Cost = dto.Cost,
                DurationMinutes = dto.DurationMinutes,
                Notes = dto.Notes,
                DelFlg = false
            };

            await _dbService.AddOrUpdateAsync(StoreName, appointment);
            return Result<AppointmentResModel>.Success(appointment, "Appointment created successfully");
        }
        catch (System.Exception ex)
        {
            return Result<AppointmentResModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<bool>> CancelAppointmentAsync(int id)
    {
        try
        {
            var appointment = await _dbService.GetByIdAsync<AppointmentResModel>(StoreName, id);
            if (appointment is null) return Result<bool>.NotFound("Appointment not found");

            appointment.Status = "Cancelled";
            await _dbService.AddOrUpdateAsync(StoreName, appointment);
            return Result<bool>.Success(true, "Appointment cancelled");
        }
        catch (System.Exception ex)
        {
            return Result<bool>.SystemError(ex.Message);
        }
    }
}
