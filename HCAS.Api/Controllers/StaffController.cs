using HCAS.Domain.Features.Staff.Models;
using HCAS.Domain.Features.Staff.Commands;
using HCAS.Domain.Features.Staff.Queries;
using HCAS.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HCAS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public StaffController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetStaffListAsync(int page = 1, int pageSize = 10, string? search = null)
        {
            var query = new GetAllStaffQuery
            {
                Page = page,
                PageSize = pageSize,
                Search = search
            };
            
            var result = await _mediator.Send(query);
      
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            var pagedResult = new
            {
                Items = result.Data?.Items,           // The staff list
                TotalCount = result.Data?.TotalCount ?? 0 // Total number of records
            };

            return Ok(pagedResult);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterStaffAsync(StaffReqModel dto)
        {
            var command = new RegisterStaffCommand
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = dto.Role,
                Username = dto.Username,
                Password = dto.Password
            };
            
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStaffAsync(StaffReqModel dto)
        {
            var command = new UpdateStaffCommand
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = dto.Role,
                Username = dto.Username,
                Password = dto.Password
            };
            
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStaffAsync(int id)
        {
            var command = new DeleteStaffCommand
            {
                Id = id
            };
            
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }
    }
}