using AutoMapper;
using DentalClinic.API.Contract;
using DentalClinic.API.Data;
using DentalClinic.API.Exceptions;
using DentalClinic.API.Models;
using DentalClinic.API.Models.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace DentalClinic.API.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    
    public class AppointmentsController : ControllerBase
    {

        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentsController(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            this._appointmentRepository = appointmentRepository;
            this._mapper = mapper;
        }

        // GET: api/Appointments
        [HttpGet("GetAll")]
        [Authorize(Roles ="Administrator")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<AppointmentsDto>>> GetAppointments()
        {
            var records = await _appointmentRepository.GetAllAsync<AppointmentsDto>();
            return Ok(records);
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User,Administrator")]
        [EnableQuery]
        public async Task<ActionResult<AppointmentsDto>> GetAppointment(int id)
        {
            var appointmentDto = await _appointmentRepository.GetAsync<AppointmentsDto>(id);
            return Ok(appointmentDto);
        }
            // GET: api/Appointments/?Page=1&PageSize=10
            [HttpGet]
        [AllowAnonymous]
        [EnableQuery]
        public async Task<ActionResult<PagedResult<AppointmentsDto>>> GetPagedAppointments([FromQuery] QueryParameters queryParameters)
        {
            var appointments = await _appointmentRepository.GetAllAsync<AppointmentsDto>(queryParameters); // Assuming the repository method takes in the desired type.
            var result = _mapper.Map<PagedResult<AppointmentsDto>>(appointments);

            return Ok(result);
        }

        // PUT: api/Appointments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "User,Administrator")]
        public async Task<IActionResult> PutAppointment(int id, AppointmentsDto appointmentDto)
        {
            if (id != appointmentDto.Id)
            {
                return BadRequest("Appointment ID in URL does not match the provided data.");
            }

            try
            {
                await _appointmentRepository.UpdateAsync(id, appointmentDto);
            }
            catch (NotFoundException)
            {
                if (!await AppointmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Appointments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "User,Administrator")]
        public async Task<ActionResult<Appointment>> PostAppointment(CreateAppointmentDto createappointmentDto)
        {
            if (createappointmentDto.Date <= DateTime.Now)
            {
                return BadRequest("Appointment date must be a future date.");
            }

            var appointmentDto = await _appointmentRepository.AddAsync<CreateAppointmentDto, AppointmentsDto>(createappointmentDto);
            return CreatedAtAction("GetAppointment", new { id = appointmentDto.Id }, appointmentDto);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _appointmentRepository.GetAsync<AppointmentsDto>(id);
            if (appointment == null)
            {
                return NotFound($"Appointment with ID {id} not found.");
            }

            if (appointment.Date <= DateTime.Now.AddHours(24))
            {
                return BadRequest("Appointments within the next 24 hours cannot be deleted.");
            }

            await _appointmentRepository.DeleteAsync(id);
            return NoContent();
        }


    private async Task<bool> AppointmentExists(int id)
        {
            return await _appointmentRepository.Exists(id);
        }
    }
}
