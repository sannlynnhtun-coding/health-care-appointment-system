using HCAS.Domain.Features.Staff.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Staff.Queries;

public class GetAllStaffQuery : IRequest<Result<PagedResult<StaffResModel>>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
}

