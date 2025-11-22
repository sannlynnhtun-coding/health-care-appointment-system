using HCAS.Domain.Features.Specializations.Models;
using HCAS.Domain.Features.Specializations.Commands;
using HCAS.Domain.Features.Specializations.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HCAS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SpecializationController(IMediator mediator)
        {
            _mediator = mediator;        
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecializationList(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] int? specializationId = null)
        {
            var query = new GetSpecializationsQuery
            {
                Page = page,
                PageSize = pageSize,
                Search = search,
                SpecializationId = specializationId
            };
            
            var result = await _mediator.Send(query);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterSpecialization(SpecializationReqModel dto)
        {
            var command = new RegisterSpecializationCommand
            {
                Name = dto.Name
            };
            
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSpecialization(int id, SpecializationReqModel dto)
        {
            var command = new UpdateSpecializationCommand
            {
                Id = id,
                Name = dto.Name
            };
            
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSpecialization(int id)
        {
            var command = new DeleteSpecializationCommand
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
