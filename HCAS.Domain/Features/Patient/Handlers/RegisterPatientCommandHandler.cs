using HCAS.Domain.Features.Patient.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Patient.Handlers;

public class RegisterPatientCommandHandler : IRequestHandler<Commands.RegisterPatientCommand, Result<PatientReqModel>>
{
    private readonly DapperService _dapper;

    public RegisterPatientCommandHandler(DapperService dapper)
    {
        _dapper = dapper;
    }

    public async Task<Result<PatientReqModel>> Handle(Commands.RegisterPatientCommand request, CancellationToken cancellationToken)
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
        return await service.RegisterPatient(dto);
    }
}

