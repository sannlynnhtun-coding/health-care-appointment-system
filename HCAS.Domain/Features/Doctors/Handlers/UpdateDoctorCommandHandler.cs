using HCAS.Database.AppDbContextModels;
using HCAS.Domain.Features.Doctors.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Doctors.Handlers;

public class UpdateDoctorCommandHandler : IRequestHandler<Commands.UpdateDoctorCommand, Result<DoctorsResModel>>
{
    private readonly AppDbContext _dbContext;

    public UpdateDoctorCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<DoctorsResModel>> Handle(Commands.UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var service = new DoctorService(_dbContext);
        var dto = new DoctorsReqModel
        {
            Name = request.Name,
            SpecializationId = request.SpecializationId
        };
        return await service.UpdateDoctorAsync(request.Id, dto);
    }
}

