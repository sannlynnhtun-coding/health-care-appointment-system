using HCAS.Database.AppDbContextModels;
using HCAS.Domain.Features.DoctorSchedule;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.DoctorSchedule.Handlers;

public class UpdateDoctorScheduleCommandHandler : IRequestHandler<Commands.UpdateDoctorScheduleCommand, Result<DoctorScheduleResModel>>
{
    private readonly DapperService _dapper;
    private readonly AppDbContext _appDbContext;

    public UpdateDoctorScheduleCommandHandler(DapperService dapper, AppDbContext appDbContext)
    {
        _dapper = dapper;
        _appDbContext = appDbContext;
    }

    public async Task<Result<DoctorScheduleResModel>> Handle(Commands.UpdateDoctorScheduleCommand request, CancellationToken cancellationToken)
    {
        var service = new DoctorScheduleService(_dapper, _appDbContext);
        var dto = new DoctorScheduleReqModel
        {
            DoctorId = request.DoctorId,
            ScheduleDate = request.ScheduleDate,
            MaxPatients = request.MaxPatients
        };
        return await service.UpdateSchedule(request.Id, dto);
    }
}

