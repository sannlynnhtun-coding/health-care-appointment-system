using HCAS.Domain.Features.Appointment.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Appointment.Commands;

public class UpdateAppointmentCommand : IRequest<Result<AppointmentResModel>>
{
    public int AppointmentId { get; set; }
    public string NewStatus { get; set; } = null!;
}

