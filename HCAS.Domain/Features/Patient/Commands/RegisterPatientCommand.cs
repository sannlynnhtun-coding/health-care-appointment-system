using HCAS.Domain.Features.Patient.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Patient.Commands;

public class RegisterPatientCommand : IRequest<Result<PatientReqModel>>
{
    public string Name { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string Gender { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
}

