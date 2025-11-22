using HCAS.Database.AppDbContextModels;
using HCAS.Domain.Features.Appointment.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Appointment.Handlers;

public class UpdateAppointmentCommandHandler : IRequestHandler<Commands.UpdateAppointmentCommand, Result<AppointmentResModel>>
{
    private readonly DapperService _dapper;
    private readonly AppDbContext _appDbContext;

    public UpdateAppointmentCommandHandler(DapperService dapper, AppDbContext appDbContext)
    {
        _dapper = dapper;
        _appDbContext = appDbContext;
    }

    public async Task<Result<AppointmentResModel>> Handle(Commands.UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var service = new AppointmentService(_dapper, _appDbContext);
        return await service.UpdateAppointment(request.AppointmentId, request.NewStatus);
    }
}

