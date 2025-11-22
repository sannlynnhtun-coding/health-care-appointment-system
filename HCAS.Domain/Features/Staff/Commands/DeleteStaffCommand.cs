using HCAS.Domain.Features.Staff.Models;
using HCAS.Shared;
using MediatR;

namespace HCAS.Domain.Features.Staff.Commands;

public class DeleteStaffCommand : IRequest<Result<StaffReqModel>>
{
    public int Id { get; set; }
}

