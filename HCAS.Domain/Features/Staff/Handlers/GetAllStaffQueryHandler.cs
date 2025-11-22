using HCAS.Domain.Features.Staff.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Staff.Handlers;

public class GetAllStaffQueryHandler : IRequestHandler<Queries.GetAllStaffQuery, Result<PagedResult<StaffResModel>>>
{
    private readonly DapperService _dapper;

    public GetAllStaffQueryHandler(DapperService dapper)
    {
        _dapper = dapper;
    }

    public async Task<Result<PagedResult<StaffResModel>>> Handle(Queries.GetAllStaffQuery request, CancellationToken cancellationToken)
    {
        var service = new StaffService(_dapper);
        return await service.GetAllStaffAsync(request.Page, request.PageSize, request.Search);
    }
}

