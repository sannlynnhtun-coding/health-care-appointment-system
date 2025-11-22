using HCAS.Domain.Features.DoctorSchedule;
using HCAS.Domain.Features.DoctorSchedule.Commands;
using HCAS.Domain.Features.DoctorSchedule.Queries;
using HCAS.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HCAS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DoctorScheduleController : BaseController
    {
        private readonly IMediator _mediator;

        public DoctorScheduleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //POST: api/v1/DoctorSchedule
        [HttpPost]
        public async Task<IActionResult> CreateDoctorSchedule([FromBody] DoctorScheduleReqModel dto)
        {
            var command = new CreateDoctorScheduleCommand
            {
                DoctorId = dto.DoctorId,
                ScheduleDate = dto.ScheduleDate,
                MaxPatients = dto.MaxPatients
            };
            
            var result = await _mediator.Send(command);
            return Excute(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctorSchedule(int id, [FromBody] DoctorScheduleReqModel dto)
        {
            var command = new UpdateDoctorScheduleCommand
            {
                Id = id,
                DoctorId = dto.DoctorId,
                ScheduleDate = dto.ScheduleDate,
                MaxPatients = dto.MaxPatients
            };
            
            var result = await _mediator.Send(command);
            return Excute(result);
        }


        //GET: api/v1/DoctorSchedule

        [HttpGet]
        public async Task<IActionResult> GetSchedules(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null
        )
        {
            var query = new GetAllSchedulesQuery();
            var result = await _mediator.Send(query);
            return Excute(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedules(int id)
        {
            var command = new DeleteDoctorScheduleCommand
            {
                Id = id
            };
            
            var result = await _mediator.Send(command);
            return Excute(result);
        }

        [HttpGet("getAvailable")]
        public async Task<IActionResult> GetAvailableSchedules()
        {
            var query = new GetAvailableSchedulesQuery();
            var result = await _mediator.Send(query);
            return Excute(result);
        }
    }
}
