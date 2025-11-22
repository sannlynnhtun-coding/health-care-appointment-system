using HCAS.Domain.Features.Specializations.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Specializations.Commands;

public class RegisterSpecializationCommand : IRequest<Result<SpecializationResModel>>
{
    public string Name { get; set; } = null!;
}

