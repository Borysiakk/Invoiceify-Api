using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Invoiceify.Domain.Contract.Requests;
using Invoiceify.Domain.Contract.Result;
using Invoiceify.Domain.Entities;

namespace Invoiceify.Infrastructure.Interface
{
    public interface IInvoiceRepository
    {
        Task<Invoice> GetInvoiceAsync(string id);
        IEnumerable<Invoice> GetInvoicesAsync();
        Task<HttpBaseResult> AddAsync(Invoice invoice);
        Task<HttpBaseResult> UpdateAsync(Invoice invoice);
    }
}