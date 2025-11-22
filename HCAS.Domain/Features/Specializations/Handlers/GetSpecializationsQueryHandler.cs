using HCAS.Database.AppDbContextModels;
using HCAS.Domain.Features.Specializations.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Specializations.Handlers;

public class GetSpecializationsQueryHandler : IRequestHandler<Queries.GetSpecializationsQuery, Result<PagedResult<SpecializationResModel>>>
{
    private readonly AppDbContext _dbContext;

    public GetSpecializationsQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<PagedResult<SpecializationResModel>>> Handle(Queries.GetSpecializationsQuery request, CancellationToken cancellationToken)
    {
        var service = new SpecializationService(_dbContext);
        return await service.GetSpecializationAsync(request.Page, request.PageSize, request.Search, request.SpecializationId);
    }
}

