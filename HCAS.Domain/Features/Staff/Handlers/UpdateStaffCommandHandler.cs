using HCAS.Domain.Features.Staff.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Staff.Handlers;

public class UpdateStaffCommandHandler : IRequestHandler<Commands.UpdateStaffCommand, Result<StaffReqModel>>
{
    private readonly DapperService _dapper;

    public UpdateStaffCommandHandler(DapperService dapper)
    {
        _dapper = dapper;
    }

    public async Task<Result<StaffReqModel>> Handle(Commands.UpdateStaffCommand request, CancellationToken cancellationToken)
    {
        var service = new StaffService(_dapper);
        var dto = new StaffReqModel
        {
            Id = request.Id,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Role = request.Role,
            Username = request.Username,
            Password = request.Password
        };
        return await service.UpdateStaffAsync(dto);
    }
}

