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
using DentalClinic.API.Models.Invoice;
using DentalClinic.API.Models.Patient;
using Microsoft.AspNetCore.Authorization;
using DentalClinic.API.Exceptions;
using DentalClinic.API.Models;
using Microsoft.AspNetCore.OData.Query;

namespace DentalClinic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class InvoicesController : ControllerBase
    {
      
        private readonly INvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public InvoicesController(INvoiceRepository invoiceRepository ,IMapper mapper )
        {
            
            this._invoiceRepository = invoiceRepository;
            this._mapper = mapper;
        }

        // GET: api/Invoices
        [HttpGet("GetAll")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoices()
        {
            var invoices = await _invoiceRepository.GetAllAsync<InvoiceDto>();
            return Ok(invoices);

        }

        // GET: api/Invoices/5
        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<ActionResult<InvoiceDto>> GetInvoice(int id)
        {
            var invoiceDto = await _invoiceRepository.GetAsync<InvoiceDto>(id);
            return Ok(invoiceDto);
        }
        // GET: api/Invoices/ByDateRange?startDate=2023-01-01&endDate=2023-12-31
        [HttpGet("ByDateRange")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoicesByDateRange(DateTime startDate, DateTime endDate)
        {
            var invoices = (await _invoiceRepository.GetAllAsync())
                        .Where(inv => inv.IssueDate >= startDate && inv.IssueDate <= endDate)
                        .ToList();

            return Ok(_mapper.Map<List<InvoiceDto>>(invoices));
        }
        // GET: api/Invoices?Page=1&PageSize=10&OrderBy=IssueDate&Search=123456
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<PagedResult<InvoiceDto>>> GetPagedInvoices([FromQuery] QueryParameters queryParameters)
        {
            var invoicesPagedResult = await _invoiceRepository.GetAllAsync<InvoiceDto>(queryParameters);
            if (invoicesPagedResult.Items == null || !invoicesPagedResult.Items.Any())
            {
                throw new BadHttpRequestException("No invoices found based on the provided criteria.");
            }
            return Ok(invoicesPagedResult);
        }
        // PUT: api/Invoices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, InvoiceDto invoiceDto)
        {

            if (id != invoiceDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _invoiceRepository.UpdateAsync<InvoiceDto>(id, invoiceDto);
            }
            catch (NotFoundException)
            {
                if (!await InvoiceExists(id))
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

        // POST: api/Invoices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Invoice>> PostInvoice(CreateInvoiceDto createInvoiceDto)
        {
            var invoiceDto = await _invoiceRepository.AddAsync<CreateInvoiceDto, InvoiceDto>(createInvoiceDto);
            return CreatedAtAction("GetInvoice", new { id = invoiceDto.Id }, invoiceDto);

        }


        // DELETE: api/Invoices/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            await _invoiceRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> InvoiceExists(int id)
        {
            return await _invoiceRepository.Exists(id);
        }
    }
}
