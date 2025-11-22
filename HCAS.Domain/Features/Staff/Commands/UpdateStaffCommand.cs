using HCAS.Domain.Features.Staff.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Staff.Commands;

public class UpdateStaffCommand : IRequest<Result<StaffReqModel>>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Role { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

