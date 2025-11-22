using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Doctors.Commands;

public class DeleteDoctorCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}

