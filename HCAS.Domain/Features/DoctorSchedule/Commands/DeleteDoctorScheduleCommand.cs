using HCAS.Domain.Features.DoctorSchedule;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.DoctorSchedule.Commands;

public class DeleteDoctorScheduleCommand : IRequest<Result<DoctorScheduleResModel>>
{
    public int Id { get; set; }
}

