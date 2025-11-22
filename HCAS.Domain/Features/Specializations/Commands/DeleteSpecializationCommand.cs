using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Specializations.Commands;

public class DeleteSpecializationCommand : IRequest<Result<bool>>
{
    public int Id { get; set; }
}

