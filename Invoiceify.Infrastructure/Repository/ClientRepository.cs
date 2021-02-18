using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Invoiceify.Domain.Contract.Requests;
using Invoiceify.Domain.Contract.Result;
using Invoiceify.Domain.Entities;
using Invoiceify.Infrastructure.Interface;
using Invoiceify.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoiceify.Infrastructure.Repository
{
    public class ClientRepository :IClientRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ClientRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public async Task<Client> GetClientAsync(string id)
        {
            return await _dbContext.Clients.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            return await _dbContext.Clients.ToListAsync();
        }

        [HttpGet]
        public IEnumerable<Client> GetClientsSlimmedAsync()
        {
            return _dbContext.Clients.AsEnumerable().Select(a => new Client()
            {
                Id = a.Id,
                Nip = a.Nip,
                City = a.City,
                Company = a.Company,
                Country = a.Country,
            });
        }

        public async Task<HttpBaseResult> DeleteAsync(string nip)
        {
            var client = await _dbContext.Clients.Where(a => a.Nip == nip).FirstOrDefaultAsync();

            if(client != null)
            {
                _dbContext.Clients.Remove(client);
                await _dbContext.SaveChangesAsync();
                return new HttpBaseResult()
                {
                    Code = HttpStatusCode.OK,
                };
            }

            return new HttpBaseResult()
            {
                Code = HttpStatusCode.NotFound,
                Errors = new[] {"Not found is product"}
            };
        }

        public async Task<HttpBaseResult> AddAsync(Client client)
        {
            try
            {
                var isClientExist = await _dbContext.Clients.AnyAsync(a => a.Nip == client.Nip);
                if (isClientExist)
                {
                    return new HttpBaseResult()
                    {
                        Success = false,
                        Code = HttpStatusCode.Conflict,
                        Errors = new[] {"A client with this id already exists "}
                    };
                }
                
                await _dbContext.Clients.AddAsync(client);
                await _dbContext.SaveChangesAsync();
                return new HttpBaseResult()
                {
                    Success = true,
                    Code = HttpStatusCode.OK,
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<HttpBaseResult> UpdateAsync(Client client)
        {
            try
            {
                var isClientExist = await _dbContext.Clients.AnyAsync(a => a.Id == client.Id);

                if (!isClientExist)
                {
                    return new HttpBaseResult()
                    {
                        Code = HttpStatusCode.NotFound,
                        Errors = new[] {"Not found is client"}
                    };
                }

                _dbContext.Update(client);
                await _dbContext.SaveChangesAsync();
            
                return new HttpBaseResult()
                {
                    Success = true,
                    Code = HttpStatusCode.OK,
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}