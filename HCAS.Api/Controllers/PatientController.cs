using HCAS.Domain.Features.Patient.Commands;
using HCAS.Domain.Features.Patient.Queries;
using HCAS.Domain.Features.Patient.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HCAS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public PatientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatientList()
        {
            var query = new GetAllPatientsQuery();
            var result = await _mediator.Send(query);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody]PatientReqModel reqModel)
        {
            var command = new RegisterPatientCommand
            {
                Name = reqModel.Name,
                DateOfBirth = reqModel.DateOfBirth,
                Gender = reqModel.Gender,
                Phone = reqModel.Phone,
                Email = reqModel.Email
            };
            
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] PatientReqModel reqModel)
        {
            var command = new UpdatePatientCommand
            {
                Id = id,
                Name = reqModel.Name,
                DateOfBirth = reqModel.DateOfBirth,
                Gender = reqModel.Gender,
                Phone = reqModel.Phone,
                Email = reqModel.Email
            };
            
            var result = await _mediator.Send(command);

            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound("Patient Not Found!");
            }
            
            return Ok(result.Data); 
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var command = new DeletePatientCommand
            {
                Id = id
            };
            
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound("Patient Not Found");
            }
            
            return Ok(result.Data);
        }
        
    }
}
