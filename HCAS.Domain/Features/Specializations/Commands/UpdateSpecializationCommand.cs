using HCAS.Domain.Features.Specializations.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Specializations.Commands;

public class UpdateSpecializationCommand : IRequest<Result<SpecializationResModel>>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

