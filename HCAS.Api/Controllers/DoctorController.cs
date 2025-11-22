using HCAS.Domain.Features.Doctors.Commands;
using HCAS.Domain.Features.Doctors.Queries;
using HCAS.Domain.Features.Doctors.Models;
using HCAS.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HCAS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DoctorController : BaseController
    {
        private readonly IMediator _mediator;

        public DoctorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/Doctor
        // Supports pagination, search, and filter by specialization
        [HttpGet]
        public async Task<IActionResult> GetDoctors(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] int? specializationId = null)
        {
            var query = new GetDoctorsQuery
            {
                Page = page,
                PageSize = pageSize,
                Search = search,
                SpecializationId = specializationId
            };
            
            var result = await _mediator.Send(query);
            return Excute(result);   
        }

        // POST: api/v1/Doctor
        [HttpPost]
        public async Task<IActionResult> RegisterDoctor([FromBody] DoctorsReqModel dto)
        {
            var command = new RegisterDoctorCommand
            {
                Name = dto.Name,
                SpecializationId = dto.SpecializationId
            };
            
            var result = await _mediator.Send(command);
            return Excute(result);
        }

        // PUT: api/v1/Doctor/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorsReqModel dto)
        {
            var command = new UpdateDoctorCommand
            {
                Id = id,
                Name = dto.Name,
                SpecializationId = dto.SpecializationId
            };
            
            var result = await _mediator.Send(command);
            return Excute(result);
        }

        // DELETE: api/v1/Doctor/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var command = new DeleteDoctorCommand
            {
                Id = id
            };
            
            var result = await _mediator.Send(command);
            return Excute(result);
        }
    }
}