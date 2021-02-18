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
using Microsoft.EntityFrameworkCore;

namespace Invoiceify.Infrastructure.Repository
{
    public class InvoiceRepository :IInvoiceRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public InvoiceRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Invoice> GetInvoiceAsync(string id)
        {
            var invoice = await _dbContext.Invoices.Where(a => a.Id == id).FirstOrDefaultAsync();
            
            if (invoice == null)
            {
                return null;
            }
            
            var buyerClient = await _dbContext.Clients.Where(a => a.Id == invoice.BuyerClientId).FirstOrDefaultAsync();
            var sellerClient = await _dbContext.Clients.Where(a => a.Id == invoice.SellerClientId).FirstOrDefaultAsync();
            
            var orders = await _dbContext.Orders.Where(a => a.InvoiceId == invoice.Id).ToListAsync();
            invoice.Orders = orders;
            return invoice;
        }

        public IEnumerable<Invoice> GetInvoicesAsync()
        {
            return _dbContext.Invoices.AsEnumerable().Select(a => new Invoice()
            {
                Id = a.Id,
                Number = a.Number,
                DateIssue = a.DateIssue,
                BuyerClientId = _dbContext.Clients.FirstOrDefault(b=>b.Id == a.BuyerClientId)?.Company,
                SellerClientId = _dbContext.Clients.FirstOrDefault(b=>b.Id == a.SellerClientId)?.Company,
            });
        }

        public async Task<HttpBaseResult> AddAsync(Invoice invoice)
        {
            bool isInvoiceExist = _dbContext.Invoices.Any(a => a.Number == invoice.Number);
            if (isInvoiceExist)
            {
                return new HttpBaseResult()
                {
                    Code = HttpStatusCode.Conflict,
                    Errors = new[] {"A invoice with this id already exists "}
                };
            }
            
            foreach (var order in invoice.Orders)
            {
                order.InvoiceId = invoice.Id;
            }

            await _dbContext.Invoices.AddAsync(invoice);
            await _dbContext.Orders.AddRangeAsync(invoice.Orders);
            await _dbContext.SaveChangesAsync();

            return new HttpBaseResult()
            {
                Success = true,
                Code = HttpStatusCode.OK,
            };
        }

        public async Task<HttpBaseResult> UpdateAsync(Invoice invoice)
        {
            bool isInvoiceExist = _dbContext.Invoices.Any(a => a.Id == invoice.Id);
            if (!isInvoiceExist)
            {
                return new HttpBaseResult()
                {
                    Code = HttpStatusCode.NotFound,
                    Errors = new[] {"Invoice not found "}
                };
            }

            var ordersId = invoice.Orders.Select(a => a.Id).ToList();
            _dbContext.Orders.AttachRange(invoice.Orders);
            foreach (var order in invoice.Orders)
            {
                _dbContext.Entry(order).State = ordersId.Contains(order.Id) ? EntityState.Modified : EntityState.Added;
            }
            
            _dbContext.Invoices.Attach(invoice);
            _dbContext.Entry(invoice).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            var orders = invoice.Orders.Select(a => a.Id);
            var removeOrders = _dbContext.Orders.Where(a => a.InvoiceId == invoice.Id)
                .Where(b => orders.All(c => c != b.Id));

            _dbContext.Orders.RemoveRange(removeOrders);
            await _dbContext.SaveChangesAsync();
            
            return new HttpBaseResult()
            {
                Success = true,
                Code = HttpStatusCode.OK,
            };
        }
    }
}