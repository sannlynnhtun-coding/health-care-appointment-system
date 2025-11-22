using HCAS.Domain.Features.Specializations.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Specializations.Queries;

public class GetSpecializationsQuery : IRequest<Result<PagedResult<SpecializationResModel>>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public int? SpecializationId { get; set; }
}

