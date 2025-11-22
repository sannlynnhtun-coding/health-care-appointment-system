using HCAS.Domain.Features.Doctors.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Doctors.Commands;

public class RegisterDoctorCommand : IRequest<Result<DoctorsResModel>>
{
    public string Name { get; set; } = null!;
    public int SpecializationId { get; set; }
}

