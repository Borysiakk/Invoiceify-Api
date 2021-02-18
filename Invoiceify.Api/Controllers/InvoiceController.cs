using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Invoiceify.Domain.Contract.Requests;
using Invoiceify.Domain.Entities;
using Invoiceify.Infrastructure.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Invoiceify.Api.Controllers
{
    [ApiController]
    [Route("api/Invoice")]
    public class InvoiceController :ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceController(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(string id)
        {
            try
            {
                var invoice = await _invoiceRepository.GetInvoiceAsync(id);
                if (invoice == null)
                {
                    return NotFound("Nie znaleziono Faktury");
                }

                return Ok(invoice);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpGet("GetInvoices")]
        public ActionResult<IEnumerable<Invoice>> GetInvoices()
        {
            try
            {
                var invoices = _invoiceRepository.GetInvoicesAsync();
                return Ok(invoices);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> AddInvoice( Invoice invoice)
        {
            try
            {
                var result = await _invoiceRepository.AddAsync(invoice);
                if (result.Code == HttpStatusCode.Conflict)
                {
                    return Conflict("Znaleziono już fakture o tym numerze ");
                }

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInvoice(Invoice invoice)
        {
            try
            {
                var result = await _invoiceRepository.UpdateAsync(invoice);
                if (result.Code == HttpStatusCode.NotFound)
                {
                    return NotFound("Znaleziono już fakture o tym numerze ");
                }

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}