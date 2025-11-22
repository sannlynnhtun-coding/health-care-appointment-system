using HCAS.Domain.Features.Doctors;
using HCAS.Domain.Features.Appointment.Models;
using HCAS.Domain.Features.Doctors.Models;
using HCAS.WebApp.Components.Pages.Doctor;
using MudBlazor;

namespace HCAS.WebApp.Components.Pages.Appointment;

public partial class Appointment
{
    List<AppointmentResponseModel> appointmentLst = new();
    List<AppointmentResponseModel> pastAppointmentLst = new();
    List<SpecializationModel> specializations = new();

    int page = 1;
    int pagePast = 1;
    int pageSize = 10;
    int totalCount = 0;
    int totalPastCount = 0;
    string doctorName = string.Empty;
    string patientName = string.Empty;
    int? selectedSpecialization = null;
    int selectedTabIndex = 0; // 0 = Current & Upcoming, 1 = Past

    private bool isNavigating = false;
    private bool isLoading = false;

    private async Task NavigateToCreateAppointment()
    {
        try
        {
            isNavigating = true;
            StateHasChanged();

            await Task.Delay(100);

            Navigation.NavigateTo("/create-appointment");
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error navigating: {ex.Message}", Severity.Error);
        }
        finally
        {
            isNavigating = false;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        // await LoadSpecializations();
        await AppointmentList(); // Load current & upcoming appointments by default
    }

    private List<IGrouping<string, AppointmentResponseModel>> groupedAppointments = new();
    private List<IGrouping<string, AppointmentResponseModel>> groupedPastAppointments = new();

    async Task AppointmentList()
    {
        isLoading = true;
        StateHasChanged();

        var result = await _appointmentService.GetAppointmentsAsync
            (page, pageSize, doctorName, patientName, includePastAppointments: false);
        if (result.IsSuccess)
        {
            appointmentLst = result.Data.Items.ToList();
            totalCount = result.Data.TotalCount;
            
            // Group appointments by Doctor and Schedule
            groupedAppointments = appointmentLst
                .GroupBy(a => $"{a.DoctorName}|{a.ScheduleId}|{a.AppointmentDate:yyyy-MM-dd}")
                .OrderBy(g => g.Key) // Order groups ascending
                .ToList();
        }
        else
        {
            appointmentLst = new List<AppointmentResponseModel>();
            groupedAppointments = new List<IGrouping<string, AppointmentResponseModel>>();
            totalCount = 0;
        }

        isLoading = false;
        StateHasChanged();
    }

    async Task PastAppointmentList()
    {
        isLoading = true;
        StateHasChanged();

        var result = await _appointmentService.GetAppointmentsAsync
            (pagePast, pageSize, doctorName, patientName, includePastAppointments: true);
        if (result.IsSuccess)
        {
            pastAppointmentLst = result.Data.Items.ToList();
            totalPastCount = result.Data.TotalCount;
            
            // Group past appointments by Doctor and Schedule
            groupedPastAppointments = pastAppointmentLst
                .GroupBy(a => $"{a.DoctorName}|{a.ScheduleId}|{a.AppointmentDate:yyyy-MM-dd}")
                .OrderByDescending(g => g.Key) // Order groups descending for past (most recent first)
                .ToList();
        }
        else
        {
            pastAppointmentLst = new List<AppointmentResponseModel>();
            groupedPastAppointments = new List<IGrouping<string, AppointmentResponseModel>>();
            totalPastCount = 0;
        }

        isLoading = false;
        StateHasChanged();
    }

    async Task OnTabChanged(int index)
    {
        selectedTabIndex = index;
        
        if (index == 0)
        {
            // Current & Upcoming tab
            await AppointmentList();
        }
        else if (index == 1)
        {
            // Past Appointments tab
            await PastAppointmentList();
        }
        
        StateHasChanged();
    }

    /*async Task LoadSpecializations()
    {
        var result = await _appointmentService.GetSpecializationsAsync();
        specializations = result?.ToList() ?? new();
    }*/

    void PageChanged(int newPage)
    {
        page = newPage;
        _ = AppointmentList();
    }

    void PastPageChanged(int newPage)
    {
        pagePast = newPage;
        _ = PastAppointmentList();
    }

    void OnDoctorNameSearchChanged(object value)
    {
        doctorName = value?.ToString() ?? string.Empty;
        page = 1;
        pagePast = 1;
        
        if (selectedTabIndex == 0)
        {
            _ = AppointmentList();
        }
        else
        {
            _ = PastAppointmentList();
        }
    }

    void OnPatientNameSearchChanged(object value)
    {
        patientName = value?.ToString() ?? string.Empty;
        page = 1;
        pagePast = 1;
        
        if (selectedTabIndex == 0)
        {
            _ = AppointmentList();
        }
        else
        {
            _ = PastAppointmentList();
        }
    }

    private async Task OpenEditDialog(AppointmentResponseModel appointment)
    {
        var parameters = new DialogParameters
        {
            { "Appointment", appointment }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<EditAppointmentDialog>("Edit Appointment Status", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            if (selectedTabIndex == 0)
            {
                await AppointmentList();
            }
            else
            {
                await PastAppointmentList();
            }
        }
    }

    private async Task DeleteAppointment(AppointmentResponseModel appointment)
    {
        // Show confirmation dialog
        var confirmationMessage = $"Are you sure you want to delete this appointment?\n\n" +
                                 $"Appointment #{appointment.AppointmentNumber}\n" +
                                 $"Patient: {appointment.PatientName}\n" +
                                 $"Doctor: {appointment.DoctorName}\n" +
                                 $"Date: {appointment.AppointmentDate:MMM dd, yyyy HH:mm}\n\n" +
                                 $"This action cannot be undone.";

        var confirmed = await DialogService.ShowMessageBox(
            "Delete Appointment",
            confirmationMessage,
            yesText: "Delete",
            cancelText: "Cancel");

        if (confirmed != true)
        {
            return;
        }

        // Delete the appointment
        var result = await _appointmentService.DeleteAppointment(appointment.Id);

        if (result.IsSuccess)
        {
            Snackbar.Add(
                $"Appointment #{appointment.AppointmentNumber} deleted successfully.",
                Severity.Success,
                config =>
                {
                    config.VisibleStateDuration = 4000;
                    config.ShowCloseIcon = true;
                });

            // Refresh the appointment list based on current tab
            if (selectedTabIndex == 0)
            {
                await AppointmentList();
            }
            else
            {
                await PastAppointmentList();
            }
        }
        else
        {
            Snackbar.Add(
                $"Failed to delete appointment: {result.Message}",
                Severity.Error,
                config =>
                {
                    config.VisibleStateDuration = 5000;
                    config.ShowCloseIcon = true;
                });
        }
    }

    private Color GetStatusColor(string status)
    {
        return status switch
        {
            "Pending" => Color.Warning,
            "Complete" => Color.Success,
            "Cancelled" => Color.Error,
            _ => Color.Default
        };
    }
}