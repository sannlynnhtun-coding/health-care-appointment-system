using HCAS.Domain.Features.Appointment.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Appointment.Queries;

public class GetAllAppointmentsQuery : IRequest<Result<IEnumerable<AppointmentResModel>>>
{
}

