using HCAS.Domain.Features.Patient.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Patient.Handlers;

public class UpdatePatientCommandHandler : IRequestHandler<Commands.UpdatePatientCommand, Result<PatientResModel>>
{
    private readonly DapperService _dapper;

    public UpdatePatientCommandHandler(DapperService dapper)
    {
        _dapper = dapper;
    }

    public async Task<Result<PatientResModel>> Handle(Commands.UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var service = new PatientService(_dapper);
        var dto = new PatientReqModel
        {
            Name = request.Name,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            Phone = request.Phone,
            Email = request.Email
        };
        return await service.UpdatePatient(dto, request.Id);
    }
}

