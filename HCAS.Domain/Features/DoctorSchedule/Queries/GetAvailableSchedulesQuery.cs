using HCAS.Domain.Features.DoctorSchedule;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.DoctorSchedule.Queries;

public class GetAvailableSchedulesQuery : IRequest<Result<IEnumerable<DoctorScheduleResModel>>>
{
}

