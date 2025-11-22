using HCAS.Domain.Features.Staff.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Staff.Handlers;

public class RegisterStaffCommandHandler : IRequestHandler<Commands.RegisterStaffCommand, Result<StaffReqModel>>
{
    private readonly DapperService _dapper;

    public RegisterStaffCommandHandler(DapperService dapper)
    {
        _dapper = dapper;
    }

    public async Task<Result<StaffReqModel>> Handle(Commands.RegisterStaffCommand request, CancellationToken cancellationToken)
    {
        var service = new StaffService(_dapper);
        var dto = new StaffReqModel
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            Role = request.Role,
            Username = request.Username,
            Password = request.Password
        };
        return await service.RegisterStaffAsync(dto);
    }
}

