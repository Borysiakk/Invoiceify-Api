using System;
using System.Net;
using System.Threading.Tasks;
using Invoiceify.Domain.Contract.Requests;
using Invoiceify.Domain.Entities;
using Invoiceify.Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Invoiceify.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Clients")]
    public class ClientController :ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Client client)
        {
            try
            {
                var result = await _clientRepository.AddAsync(client);
                if (result.Code == HttpStatusCode.Conflict)
                {
                    return Conflict($"Istnieje już klient o podanym NIP"); 
                }
                return Ok("Klient został dodany");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Client client)
        {
            try
            {
                var result  = await _clientRepository.UpdateAsync(client);
                if (result.Code == HttpStatusCode.NotFound)
                {
                    return NotFound($"Nie znaleziono produktu o kodzie : {client.Nip}");
                }

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProducts(string id)
        {
            try
            {
                var result  = await _clientRepository.DeleteAsync(id);
                if (result.Code == HttpStatusCode.NotFound)
                {
                    return NotFound($"Nie znaleziono produktu o kodzie : {id}");
                }

                return Ok();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(string id)
        {
            try
            {
                Client client = await _clientRepository.GetClientAsync(id);
                if (client == null)
                {
                    return NotFound("Nie znaleziono klienta!");
                }
                return client;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            try
            {
                return Ok(await _clientRepository.GetClientsAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpGet("ClientsSlimmed")]
        public IActionResult GetClientsSlimmed()
        {
            try
            {
                return Ok(_clientRepository.GetClientsSlimmedAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}