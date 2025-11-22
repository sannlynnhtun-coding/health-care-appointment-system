using HCAS.Database.AppDbContextModels;
using HCAS.Domain.Features.DoctorSchedule;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.DoctorSchedule.Handlers;

public class GetAllSchedulesQueryHandler : IRequestHandler<Queries.GetAllSchedulesQuery, Result<IEnumerable<DoctorScheduleResModel>>>
{
    private readonly DapperService _dapper;
    private readonly AppDbContext _appDbContext;

    public GetAllSchedulesQueryHandler(DapperService dapper, AppDbContext appDbContext)
    {
        _dapper = dapper;
        _appDbContext = appDbContext;
    }

    public async Task<Result<IEnumerable<DoctorScheduleResModel>>> Handle(Queries.GetAllSchedulesQuery request, CancellationToken cancellationToken)
    {
        var service = new DoctorScheduleService(_dapper, _appDbContext);
        return await service.GetAllSchedules();
    }
}

