using HCAS.Domain.Features.DoctorSchedule;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.DoctorSchedule.Commands;

public class CreateDoctorScheduleCommand : IRequest<Result<DoctorScheduleResModel>>
{
    public int DoctorId { get; set; }
    public DateTime ScheduleDate { get; set; }
    public int MaxPatients { get; set; }
}

