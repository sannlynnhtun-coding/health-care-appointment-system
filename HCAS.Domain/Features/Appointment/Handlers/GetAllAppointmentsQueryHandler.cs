using HCAS.Database.AppDbContextModels;
using HCAS.Domain.Features.Appointment.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Appointment.Handlers;

public class GetAllAppointmentsQueryHandler : IRequestHandler<Queries.GetAllAppointmentsQuery, Result<IEnumerable<AppointmentResModel>>>
{
    private readonly DapperService _dapper;
    private readonly AppDbContext _appDbContext;

    public GetAllAppointmentsQueryHandler(DapperService dapper, AppDbContext appDbContext)
    {
        _dapper = dapper;
        _appDbContext = appDbContext;
    }

    public async Task<Result<IEnumerable<AppointmentResModel>>> Handle(Queries.GetAllAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var service = new AppointmentService(_dapper, _appDbContext);
        return await service.GetAllAppointments();
    }
}

