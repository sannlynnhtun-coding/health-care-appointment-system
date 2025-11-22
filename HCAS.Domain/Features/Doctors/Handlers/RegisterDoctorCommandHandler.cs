using HCAS.Database.AppDbContextModels;
using HCAS.Domain.Features.Doctors.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Doctors.Handlers;

public class RegisterDoctorCommandHandler : IRequestHandler<Commands.RegisterDoctorCommand, Result<DoctorsResModel>>
{
    private readonly AppDbContext _dbContext;

    public RegisterDoctorCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<DoctorsResModel>> Handle(Commands.RegisterDoctorCommand request, CancellationToken cancellationToken)
    {
        var service = new DoctorService(_dbContext);
        var dto = new DoctorsReqModel
        {
            Name = request.Name,
            SpecializationId = request.SpecializationId
        };
        return await service.RegisterDoctorAsync(dto);
    }
}

