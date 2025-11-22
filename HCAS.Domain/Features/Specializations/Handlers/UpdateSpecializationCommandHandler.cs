using HCAS.Database.AppDbContextModels;
using HCAS.Domain.Features.Specializations.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Specializations.Handlers;

public class UpdateSpecializationCommandHandler : IRequestHandler<Commands.UpdateSpecializationCommand, Result<SpecializationResModel>>
{
    private readonly AppDbContext _dbContext;

    public UpdateSpecializationCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<SpecializationResModel>> Handle(Commands.UpdateSpecializationCommand request, CancellationToken cancellationToken)
    {
        var service = new SpecializationService(_dbContext);
        var dto = new SpecializationReqModel
        {
            Name = request.Name
        };
        return await service.UpdateSpecializationAsync(request.Id, dto);
    }
}

