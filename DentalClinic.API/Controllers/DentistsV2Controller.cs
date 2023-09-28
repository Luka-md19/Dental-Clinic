using AutoMapper;
using DentalClinic.API.Contract;
using DentalClinic.API.Data;
using DentalClinic.API.Exceptions;
using DentalClinic.API.Models;
using DentalClinic.API.Models.Dentists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

[Route("api/v{version:apiVersion}/dentists")]
[ApiController]
[ApiVersion("2.0")]

public class DentistsV2Controller : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IDentistRepository _dentistRepository;
    private readonly ILogger<DentistsV2Controller> _logger;

    public DentistsV2Controller(IMapper mapper, IDentistRepository dentistRepository , ILogger<DentistsV2Controller> logger)
    {
        this._mapper = mapper;
        this._dentistRepository = dentistRepository;
        this._logger = logger;
    }

    [HttpGet]
    [AllowAnonymous] // Everyone can access this
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<GetDentistDto>>> GetDentists()
    {
        var dentist = await _dentistRepository.GetAllAsync();
        var records = _mapper.Map<List<GetDentistDto>>(dentist);
        return Ok(records);
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // Everyone can access this
    [EnableQuery]
    public async Task<ActionResult<DentistDto>> GetDentist(int id)
    {
        var dentist = await _dentistRepository.GetDetails(id);
        if (dentist == null)
        {
           throw new NotFoundException(nameof(GetDentist), id);
          
        }
        var dentistDto = _mapper.Map<DentistDto>(dentist);
        return Ok(dentistDto);
    }

    [HttpGet("specialization/{specialization}")]
    [AllowAnonymous] // Everyone can access this
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<DentistDto>>> GetDentistsBySpecialization(string specialization)
    {
        var dentists = await _dentistRepository.FindBySpecializationAsync(specialization);
        if (!dentists.Any())
        {
            throw new BadRequestException("Specialization is required.");
        }
        var dentistDtos = _mapper.Map<IEnumerable<DentistDto>>(dentists);
        return Ok(dentistDtos);
    }

    [HttpGet("{id}/hasappointments")]
    [EnableQuery]
    public async Task<ActionResult<bool>> DentistHasAppointments(int id)
    {
        if (!await _dentistRepository.Exists(id))
        {
            throw new NotFoundException(nameof(DentistHasAppointments), id);
        }
        var hasAppointments = await _dentistRepository.HasAppointmentsAsync(id);
        return Ok(hasAppointments);
    }
    // GET: api/v2/dentists?Page=1&PageSize=10&OrderBy=name&Search=John
    [HttpGet("GetAll")]
    [AllowAnonymous] // Everyone can access this
    [EnableQuery]
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
            
            throw new BadRequestException($"Mismatch between URL ID ({id}) and DTO ID ({updateDentistDto.Id}).");
        }

        var dentist = await _dentistRepository.GetAsync(id);
        if (dentist == null)
        {
            throw new NotFoundException(nameof(GetDentists), id);
        }
        _mapper.Map(updateDentistDto, dentist);

        try
        {
            await _dentistRepository.UpdateAsync(dentist);
        }
        catch (DbUpdateConcurrencyException)
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
        if (string.IsNullOrEmpty(createDentistDto.Name))
        {
            throw new BadRequestException("Dentist name is required.");
        }
        var dentist = _mapper.Map<Dentist>(createDentistDto);
        await _dentistRepository.AddAsync(dentist);
        return CreatedAtAction("GetDentist", new { id = dentist.Id }, dentist);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteDentist(int id)
    {
        var dentist = await _dentistRepository.GetAsync(id);
        if (dentist == null)
        {
            throw new NotFoundException(nameof(GetDentists), id);
        }

        await _dentistRepository.DeleteAsync(id);
        return NoContent();
    }

    private async Task<bool> DentistExists(int id)
    {
        return await _dentistRepository.Exists(id);
    }
}
