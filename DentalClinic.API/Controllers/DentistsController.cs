using AutoMapper;
using DentalClinic.API.Contract;
using DentalClinic.API.Data;
using DentalClinic.API.Exceptions;
using DentalClinic.API.Models;
using DentalClinic.API.Models.Dentists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/v{version:apiVersion}/dentists")]
[ApiController]
[ApiVersion("1.0", Deprecated = true)]

public class DentistsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IDentistRepository _dentistRepository;

    public DentistsController(IMapper mapper, IDentistRepository dentistRepository)
    {
        this._mapper = mapper;
        this._dentistRepository = dentistRepository;
    }

    [HttpGet("GetAll")]
    [AllowAnonymous] // Everyone can access this
    public async Task<ActionResult<IEnumerable<GetDentistDto>>> GetDentists()
    {
        var records = await _dentistRepository.GetAllAsync<GetDentistDto>();
        return Ok(records);
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // Everyone can access this
    public async Task<ActionResult<DentistDto>> GetDentist(int id)
    {
        var dentistDto = await _dentistRepository.GetDetails(id);
        return Ok(dentistDto);
    }

    [HttpGet("specialization/{specialization}")]
    [AllowAnonymous] // Everyone can access this
    public async Task<ActionResult<IEnumerable<DentistDto>>> GetDentistsBySpecialization(string specialization)
    {
        var dentists = await _dentistRepository.FindBySpecializationAsync(specialization);
        if (!dentists.Any())
        {
            return NotFound();
        }
        var dentistDtos = _mapper.Map<IEnumerable<DentistDto>>(dentists);
        return Ok(dentistDtos);
    }

    [HttpGet("{id}/hasappointments")]
    public async Task<ActionResult<bool>> DentistHasAppointments(int id)
    {
        var hasAppointments = await _dentistRepository.HasAppointmentsAsync(id);
        return Ok(hasAppointments);
    }

    // GET: api/dentists/Get?Page=1&PageSize=10&OrderBy=name&Search=John
    [HttpGet]
    [AllowAnonymous] // Everyone can access this
    public async Task<ActionResult<PagedResult<GetDentistDto>>> GetDentists([FromQuery] QueryParameters queryParameters)
    {
        var dentistsPagedResult = await _dentistRepository.GetAllAsync<GetDentistDto>(queryParameters);
        if (dentistsPagedResult.Items == null || !dentistsPagedResult.Items.Any())
        {
            throw new BadHttpRequestException("No dentists found based on the provided criteria.");
        }
        return Ok(dentistsPagedResult);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> PutDentist(int id, UpdateDentistDto updateDentistDto)
    {
        if (id != updateDentistDto.Id)
        {
            return BadRequest();
        }

        try
        {
            await _dentistRepository.UpdateAsync(id, updateDentistDto);

        }
        catch (NotFoundException)
        {
            if (!await DentistExists(id))
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

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<Dentist>> PostDentist(CreateDentistDto createDentistDto)
    {
        var dentistDto = await _dentistRepository.AddAsync<CreateDentistDto, GetDentistDto>(createDentistDto);
        return CreatedAtAction("GetDentist", new { id = dentistDto.Id }, dentistDto);
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteDentist(int id)
    {
        await _dentistRepository.DeleteAsync(id);
        return NoContent();
    }

    private async Task<bool> DentistExists(int id)
    {
        return await _dentistRepository.Exists(id);
    }
}