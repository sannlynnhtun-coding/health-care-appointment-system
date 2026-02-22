namespace HCAS.WasmApp.Models.Appointments;

public class AppointmentReqModel
{
    public int ScheduleId { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public System.DateTime AppointmentDate { get; set; }
    public int AppointmentNumber { get; set; }
    public string Status { get; set; } = "Pending";
    public decimal Cost { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public string? Notes { get; set; }
}

public class AppointmentResModel
{
    public int Id { get; set; }
    public int ScheduleId { get; set; }
    public int? DoctorId { get; set; }
    public string? DoctorName { get; set; }
    public int PatientId { get; set; }
    public string? PatientName { get; set; }
    public System.DateTime? AppointmentDate { get; set; }
    public int AppointmentNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public int DurationMinutes { get; set; }
    public string? Notes { get; set; }
    public bool DelFlg { get; set; }
}

public class DoctorScheduleResModel
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public string DayOfWeek { get; set; } = string.Empty;
    public System.TimeSpan StartTime { get; set; }
    public System.TimeSpan EndTime { get; set; }
}
