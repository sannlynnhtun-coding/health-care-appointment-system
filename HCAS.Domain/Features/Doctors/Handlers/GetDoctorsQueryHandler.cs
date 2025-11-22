using HCAS.Database.AppDbContextModels;
using HCAS.Domain.Features.Doctors.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Doctors.Handlers;

public class GetDoctorsQueryHandler : IRequestHandler<Queries.GetDoctorsQuery, Result<PagedResult<DoctorsResModel>>>
{
    private readonly AppDbContext _dbContext;

    public GetDoctorsQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<PagedResult<DoctorsResModel>>> Handle(Queries.GetDoctorsQuery request, CancellationToken cancellationToken)
    {
        var service = new DoctorService(_dbContext);
        return await service.GetDoctorsAsync(request.Page, request.PageSize, request.Search, request.SpecializationId);
    }
}

