using HCAS.Domain.Features.Patient.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Patient.Handlers;

public class DeletePatientCommandHandler : IRequestHandler<Commands.DeletePatientCommand, Result<PatientReqModel>>
{
    private readonly DapperService _dapper;

    public DeletePatientCommandHandler(DapperService dapper)
    {
        _dapper = dapper;
    }

    public async Task<Result<PatientReqModel>> Handle(Commands.DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var service = new PatientService(_dapper);
        return await service.DeletePatient(request.Id);
    }
}

