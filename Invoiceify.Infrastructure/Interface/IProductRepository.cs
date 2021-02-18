using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Invoiceify.Domain.Contract.Requests;
using Invoiceify.Domain.Contract.Result;
using Invoiceify.Domain.Entities;

namespace Invoiceify.Infrastructure.Interface
{
    public interface IProductRepository
    {
        public Task<Product> GetProductAsync(string code);
        public Task<IEnumerable<Product>> GetProductsAsync();

        public IEnumerable<Product> GetProductsSlimmedAsync();
        public Task<HttpBaseResult> DeleteProductAsync(string code);
        public Task<HttpBaseResult> AddAsync(ProductRegistrationRequest productRegistration);
        public Task<HttpBaseResult> UpdateAsync(ProductRegistrationRequest productRegistrationRequest);
    }
}