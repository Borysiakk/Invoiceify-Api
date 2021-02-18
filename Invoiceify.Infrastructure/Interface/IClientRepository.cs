
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Invoiceify.Domain.Contract.Requests;
using Invoiceify.Domain.Contract.Result;
using Invoiceify.Domain.Entities;

namespace Invoiceify.Infrastructure.Interface
{
    public interface IClientRepository
    {
        public Task<Client> GetClientAsync(string id);
        public Task<IEnumerable<Client>> GetClientsAsync();
        public IEnumerable<Client> GetClientsSlimmedAsync ();
        public Task<HttpBaseResult> DeleteAsync(string code);
        public Task<HttpBaseResult> AddAsync(Client client);
        public Task<HttpBaseResult> UpdateAsync(Client client);
    }
}