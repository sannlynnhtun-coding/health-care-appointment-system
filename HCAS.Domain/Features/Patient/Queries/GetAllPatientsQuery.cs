using HCAS.Domain.Features.Patient.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Patient.Queries;

public class GetAllPatientsQuery : IRequest<Result<List<PatientResModel>>>
{
}

