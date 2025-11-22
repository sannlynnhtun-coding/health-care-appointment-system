using HCAS.Domain.Features.Doctors.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Doctors.Queries;

public class GetDoctorsQuery : IRequest<Result<PagedResult<DoctorsResModel>>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public int? SpecializationId { get; set; }
}

