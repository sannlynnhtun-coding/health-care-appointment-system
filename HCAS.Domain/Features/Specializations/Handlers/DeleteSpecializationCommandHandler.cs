using HCAS.Database.AppDbContextModels;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Specializations.Handlers;

public class DeleteSpecializationCommandHandler : IRequestHandler<Commands.DeleteSpecializationCommand, Result<bool>>
{
    private readonly AppDbContext _dbContext;

    public DeleteSpecializationCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<bool>> Handle(Commands.DeleteSpecializationCommand request, CancellationToken cancellationToken)
    {
        var service = new SpecializationService(_dbContext);
        return await service.DeleteSpecializationAsync(request.Id);
    }
}

