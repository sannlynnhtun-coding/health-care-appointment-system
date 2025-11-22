using HCAS.Database.AppDbContextModels;
using HCAS.Domain.Features.DoctorSchedule;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.DoctorSchedule.Handlers;

public class DeleteDoctorScheduleCommandHandler : IRequestHandler<Commands.DeleteDoctorScheduleCommand, Result<DoctorScheduleResModel>>
{
    private readonly DapperService _dapper;
    private readonly AppDbContext _appDbContext;

    public DeleteDoctorScheduleCommandHandler(DapperService dapper, AppDbContext appDbContext)
    {
        _dapper = dapper;
        _appDbContext = appDbContext;
    }

    public async Task<Result<DoctorScheduleResModel>> Handle(Commands.DeleteDoctorScheduleCommand request, CancellationToken cancellationToken)
    {
        var service = new DoctorScheduleService(_dapper, _appDbContext);
        return await service.DeleteSchedule(request.Id);
    }
}

