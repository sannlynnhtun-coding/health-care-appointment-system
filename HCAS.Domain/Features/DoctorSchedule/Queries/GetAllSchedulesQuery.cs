using HCAS.Domain.Features.DoctorSchedule;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.DoctorSchedule.Queries;

public class GetAllSchedulesQuery : IRequest<Result<IEnumerable<DoctorScheduleResModel>>>
{
}

