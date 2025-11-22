using HCAS.Domain.Features.Staff.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Staff.Handlers;

public class DeleteStaffCommandHandler : IRequestHandler<Commands.DeleteStaffCommand, Result<StaffReqModel>>
{
    private readonly DapperService _dapper;

    public DeleteStaffCommandHandler(DapperService dapper)
    {
        _dapper = dapper;
    }

    public async Task<Result<StaffReqModel>> Handle(Commands.DeleteStaffCommand request, CancellationToken cancellationToken)
    {
        var service = new StaffService(_dapper);
        return await service.DeleteStaffAsync(request.Id);
    }
}

