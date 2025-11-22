using HCAS.Domain.Features.Appointment.Commands;
using HCAS.Domain.Features.Appointment.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HCAS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointmentList()
        {
            var query = new GetAllAppointmentsQuery();
            var result = await _mediator.Send(query);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetAppoinmentById(int id)
        //{
        //    var appoinment = await _appointment.GetAppoinmentById(id);
        //    return Ok(appoinment);
        //}

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(int patientId, int scheduleId)
        {
            var command = new CreateAppointmentCommand
            {
                PatientId = patientId,
                ScheduleId = scheduleId
            };
            
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAppointmentStatus(int appointmentId, string newStatus)
        {
            var command = new UpdateAppointmentCommand
            {
                AppointmentId = appointmentId,
                NewStatus = newStatus
            };
            
            var result = await _mediator.Send(command);
            
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            
            return Ok(result.Data);
        }
    }
}