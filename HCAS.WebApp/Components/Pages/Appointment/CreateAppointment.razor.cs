using HCAS.Domain.Features.DoctorSchedule.Models;
using HCAS.Domain.Features.Patient.Models;
using MudBlazor;

namespace HCAS.WebApp.Components.Pages.Appointment;

public partial class CreateAppointment
{
    List<DoctorScheduleResponseModel> scheduleLst = new();
    List<PatientResModel> patientList = new();

    int? selectedPatientId = null;
    string selectedPatientName = string.Empty;
    int? selectedScheduleId = null;

    int page = 1;
    int pageSize = 10;
    int totalCount = 0;
    bool isCreating = false;

    private MudTable<DoctorScheduleResponseModel> mudTable;

    private bool IsCreateButtonEnabled => selectedPatientId is > 0 && selectedScheduleId is > 0;

    protected override async Task OnInitializedAsync()
    {
        await Task.WhenAll(PatientList(), AvailableScheduleList());
        
        // Show helpful message if no schedules are available
        if (scheduleLst == null || !scheduleLst.Any())
        {
            Snackbar.Add(
                "No doctor schedules found. Please create doctor schedules first before booking appointments.", 
                Severity.Warning,
                config => {
                    config.VisibleStateDuration = 8000;
                    config.ShowCloseIcon = true;
                });
        }
    }

    async Task AvailableScheduleList()
    {
        var result = await _doctorScheduleService.GetDoctorSchedulesAsync(page, pageSize);
        if (result.IsSuccess && result.Data != null)
        {
            scheduleLst = result.Data.Items?.ToList() ?? new List<DoctorScheduleResponseModel>();
            totalCount = result.Data.TotalCount;
        }
        else
        {
            scheduleLst = new List<DoctorScheduleResponseModel>();
            totalCount = 0;
            if (!result.IsSuccess && !string.IsNullOrEmpty(result.Message))
            {
                // Don't show error snackbar if it's just "No schedules found" - we handle that in the UI
                if (!result.Message.Contains("No schedules found", StringComparison.OrdinalIgnoreCase))
                {
                    Snackbar.Add($"Failed to load doctor schedules: {result.Message}", Severity.Error);
                }
            }
        }

        StateHasChanged();
    }

    async Task PatientList()
    {
        var result = await _patientService.GetAllPatient();
        if (result.IsSuccess && result.Data != null)
        {
            patientList = result.Data.ToList();
            
            if (!patientList.Any())
            {
                Snackbar.Add(
                    "No patients found. Please create patients first.", 
                    Severity.Info,
                    config => {
                        config.VisibleStateDuration = 5000;
                        config.ShowCloseIcon = true;
                    });
            }
        }
        else
        {
            patientList = new List<PatientResModel>();
            Snackbar.Add("Failed to load patients", Severity.Error);
        }

        StateHasChanged();
    }

    async Task PageChanged(int newPage)
    {
        page = newPage;
        await AvailableScheduleList();
        
        // Clear selection when changing pages
        selectedScheduleId = null;
        StateHasChanged();
    }

    void OnPatientSelectionChanged(object value)
    {
        selectedPatientId = value as int?;
        var selectedPatient = patientList.FirstOrDefault(p => p.Id == selectedPatientId);
        selectedPatientName = selectedPatient?.Name ?? string.Empty;
        StateHasChanged();
    }

    private void HandleRowClick(TableRowClickEventArgs<DoctorScheduleResponseModel> args)
    {
        var selectedRow = args.Item;
        if (selectedRow != null)
        {
            if (selectedRow.AvailableSlots > 0)
            {
                selectedScheduleId = selectedRow.Id;
                Snackbar.Add(
                    $"Schedule selected: {selectedRow.DoctorName} - {selectedRow.ScheduleDate:MM/dd/yyyy HH:mm}", 
                    Severity.Success,
                    config => {
                        config.VisibleStateDuration = 3000;
                        config.ShowCloseIcon = true;
                    });
            }
            else
            {
                Snackbar.Add(
                    "This schedule is full. Please select another schedule with available slots.", 
                    Severity.Warning,
                    config => {
                        config.VisibleStateDuration = 4000;
                        config.ShowCloseIcon = true;
                    });
            }
            StateHasChanged();
        }
    }

    private string GetRowClass(DoctorScheduleResponseModel schedule, int rowNumber)
    {
        var classes = new List<string>();
        classes.Add("cursor-pointer");
        if (schedule.AvailableSlots <= 0)
        {
            classes.Add("disabled-row");
        }

        if (selectedScheduleId == schedule.Id)
        {
            classes.Add("selected-row");
        }

        return string.Join(" ", classes);
    }

    private async Task CreateAppointmentForPatient()
    {
        if (!IsCreateButtonEnabled)
        {
            Snackbar.Add("Please select both a patient and an available schedule", Severity.Warning);
            return;
        }

        var selectedSchedule = scheduleLst.FirstOrDefault(s => s.Id == selectedScheduleId);
        if (selectedSchedule == null || selectedSchedule.AvailableSlots <= 0)
        {
            Snackbar.Add(
                "The selected schedule is no longer available! Please refresh and select another schedule.", 
                Severity.Error);
            
            // Refresh the schedule list
            selectedScheduleId = null;
            await AvailableScheduleList();
            return;
        }

        if (selectedPatientId == null || selectedPatientId <= 0)
        {
            Snackbar.Add("Please select a patient first", Severity.Warning);
            return;
        }

        // Get the next available appointment time (20-minute slot)
        var nextAvailableTime = await _appointmentService.GetNextAvailableAppointmentTime(selectedScheduleId.Value);
        
        if (!nextAvailableTime.HasValue)
        {
            Snackbar.Add(
                "No available time slots for this schedule. Please select another schedule.", 
                Severity.Error);
            selectedScheduleId = null;
            await AvailableScheduleList();
            return;
        }

        // Show confirmation dialog with the actual time slot
        var selectedPatient = patientList.FirstOrDefault(p => p.Id == selectedPatientId);
        var confirmationMessage = $"Create appointment for {selectedPatient?.Name ?? "Patient"} with Dr. {selectedSchedule.DoctorName}?\n\n" +
                                 $"Date: {nextAvailableTime.Value:MMM dd, yyyy}\n" +
                                 $"Time: {nextAvailableTime.Value:HH:mm} - {nextAvailableTime.Value.AddMinutes(20):HH:mm} (20 minutes)\n\n" +
                                 $"Note: Each appointment takes 20 minutes. If appointments at 12:00 PM and 12:20 PM exist, the next slot will be 12:40 PM.";
        
        var confirmed = await DialogService.ShowMessageBox(
            "Confirm Appointment",
            confirmationMessage,
            yesText: "Create Appointment", 
            cancelText: "Cancel");

        if (confirmed != true)
        {
            return;
        }

        isCreating = true;
        StateHasChanged();

        var result = await _appointmentService.CreateAppointment(selectedPatientId.Value, selectedScheduleId.Value);

        if (result.IsSuccess && result.Data != null)
        {
            var appointmentTime = result.Data.AppointmentDate.HasValue 
                ? result.Data.AppointmentDate.Value 
                : DateTime.Now;
            
            Snackbar.Add(
                $"✅ Appointment created successfully!\n" +
                $"📅 Time: {appointmentTime:HH:mm} - {appointmentTime.AddMinutes(20):HH:mm} (20 min slot)\n" +
                $"🔢 Appointment #: {result.Data.AppointmentNumber}", 
                Severity.Success,
                config => {
                    config.VisibleStateDuration = 6000;
                    config.ShowCloseIcon = true;
                });

            // Reset form
            selectedPatientId = null;
            selectedPatientName = string.Empty;
            selectedScheduleId = null;

            // Refresh schedule list to update availability
            await AvailableScheduleList();
        }
        else
        {
            Snackbar.Add(
                $"Failed to create appointment: {result.Message}", 
                Severity.Error,
                config => {
                    config.VisibleStateDuration = 6000;
                    config.ShowCloseIcon = true;
                });
        }

        isCreating = false;
        StateHasChanged();
    }

    private void NavigateToDoctorSchedule()
    {
        Navigation.NavigateTo("/DoctorSchedule");
    }
}