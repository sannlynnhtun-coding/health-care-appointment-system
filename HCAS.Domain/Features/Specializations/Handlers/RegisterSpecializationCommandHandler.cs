using HCAS.Database.AppDbContextModels;
using HCAS.Domain.Features.Specializations.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Specializations.Handlers;

public class RegisterSpecializationCommandHandler : IRequestHandler<Commands.RegisterSpecializationCommand, Result<SpecializationResModel>>
{
    private readonly AppDbContext _dbContext;

    public RegisterSpecializationCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<SpecializationResModel>> Handle(Commands.RegisterSpecializationCommand request, CancellationToken cancellationToken)
    {
        var service = new SpecializationService(_dbContext);
        var dto = new SpecializationReqModel
        {
            Name = request.Name
        };
        return await service.RegisterSpecializationAsync(dto);
    }
}

