using HCAS.Domain.Features.Patient.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Patient.Commands;

public class DeletePatientCommand : IRequest<Result<PatientReqModel>>
{
    public int Id { get; set; }
}

