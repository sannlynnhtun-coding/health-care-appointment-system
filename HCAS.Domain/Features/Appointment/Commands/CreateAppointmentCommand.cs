using HCAS.Domain.Features.Appointment.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Appointment.Commands;

public class CreateAppointmentCommand : IRequest<Result<AppointmentResModel>>
{
    public int PatientId { get; set; }
    public int ScheduleId { get; set; }
}

