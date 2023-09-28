using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DentalClinic.API.Data;
using DentalClinic.API.Contract;
using AutoMapper;
using DentalClinic.API.Models.Patient;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Authorization;
using DentalClinic.API.Exceptions;
using DentalClinic.API.Models;
using System.Drawing.Printing;
using Microsoft.AspNetCore.OData.Query;
using DentalClinic.API.Models.Dentists;
using DentalClinic.API.Repository;

namespace DentalClinic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PatientsController : ControllerBase
    {
   
        private readonly IPatientsRepository _patientsRepository;
        private readonly IMapper _mapper;

        public PatientsController(IPatientsRepository patientsRepository,IMapper mapper)
        {
          
            this._patientsRepository = patientsRepository;
            this._mapper = mapper;
        }

        // GET: api/Patients
        [HttpGet("GetAll")]
        [AllowAnonymous]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<GetPatientDto>>> GetPatients()
        {
            var patients = await _patientsRepository.GetAllAsync<GetPatientDto>();
            return Ok(patients);
        }

        // GET: api/Patients/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User,Administrator")]
        [EnableQuery]
        public async Task<ActionResult<PatientDto>> GetPatient(int id)
        {
            var patientDto = await _patientsRepository.GetPatientsDetails(id);
            return Ok(patientDto);
        }
        
        // GET: api/Patients/AppointmentsAfter/2023-08-16
        [HttpGet("AppointmentsAfter/{date}")]
        [Authorize(Roles = "Administrator")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatientsWithAppointmentsAfter(DateTime date)
        {
            if (date > DateTime.Now)
            {
                throw new BadRequestException("Date cannot be in the future.");
            }
            var patients = await _patientsRepository.GetPatientsWithAppointmentsAfterAsync(date);
            return Ok(_mapper.Map<List<PatientDto>>(patients));
        }
        //GET: api/Patients?Page=1&PageSize=10&OrderBy=name&Search=john
        [HttpGet]
        [AllowAnonymous]
        [EnableQuery]
        public async Task<ActionResult<PagedResult<GetPatientDto>>> GetPagedPatients([FromQuery] QueryParameters queryParameters)
        {
            var patientsPagedResult = await _patientsRepository.GetAllAsync<GetPatientDto>(queryParameters);
            if (patientsPagedResult.Items == null || !patientsPagedResult.Items.Any())
            {
                throw new BadHttpRequestException("No patients found based on the provided criteria.");
            }
            return Ok(patientsPagedResult);
        }

        // PUT: api/Patients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "User,Administrator")]

        public async Task<IActionResult> PutPatient(int id, PatientDto patientDto)
        {
            if (id != patientDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _patientsRepository.UpdateAsync(id, patientDto);
            }
            catch (NotFoundException)
            {
                if (!await PatientExists(id))
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

        // POST: api/Patients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrator")] 
        public async Task<ActionResult<Patient>> PostPatient(CreatePatientDto patientdto)
        {
            var patientDto = await _patientsRepository.AddAsync<CreatePatientDto, PatientDto>(patientdto);
            return CreatedAtAction("GetPatient", new { id = patientDto.Id }, patientDto);
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            await _patientsRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> PatientExists(int id)
        {
            return await _patientsRepository.Exists(id);
        }
    }
}
