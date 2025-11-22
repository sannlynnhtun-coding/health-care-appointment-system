using HCAS.Database.AppDbContextModels;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Doctors.Handlers;

public class DeleteDoctorCommandHandler : IRequestHandler<Commands.DeleteDoctorCommand, Result<bool>>
{
    private readonly AppDbContext _dbContext;

    public DeleteDoctorCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<bool>> Handle(Commands.DeleteDoctorCommand request, CancellationToken cancellationToken)
    {
        var service = new DoctorService(_dbContext);
        return await service.DeleteDoctorAsync(request.Id);
    }
}

