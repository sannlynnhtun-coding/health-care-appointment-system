using HCAS.Domain.Features.Doctors.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Doctors.Commands;

public class UpdateDoctorCommand : IRequest<Result<DoctorsResModel>>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int SpecializationId { get; set; }
}

