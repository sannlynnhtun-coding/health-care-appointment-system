using HCAS.Domain.Features.Patient.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Patient.Handlers;

public class GetAllPatientsQueryHandler : IRequestHandler<Queries.GetAllPatientsQuery, Result<List<PatientResModel>>>
{
    private readonly DapperService _dapper;

    public GetAllPatientsQueryHandler(DapperService dapper)
    {
        _dapper = dapper;
    }

    public async Task<Result<List<PatientResModel>>> Handle(Queries.GetAllPatientsQuery request, CancellationToken cancellationToken)
    {
        var service = new PatientService(_dapper);
        return await service.GetAllPatient();
    }
}

