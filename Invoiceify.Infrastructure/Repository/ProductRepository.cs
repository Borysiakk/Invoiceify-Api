using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Invoiceify.Domain.Contract.Requests;
using Invoiceify.Domain.Contract.Result;
using Invoiceify.Domain.Entities;
using Invoiceify.Infrastructure.Interface;
using Invoiceify.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Invoiceify.Infrastructure.Repository
{
    public class ProductRepository :IProductRepository
    {
        private readonly IHostEnvironment _environment;
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(IHostEnvironment environment, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        public async Task<HttpBaseResult> AddAsync(ProductRegistrationRequest productRegistration)
        {
            try
            {
                var isProductExist = _dbContext.Products.Any(a => a.Code == productRegistration.Code);

                if (isProductExist)
                {
                    return new HttpBaseResult()
                    {
                        Code = HttpStatusCode.Conflict,
                        Errors = new[] {"A product with this id already exists "}
                    };
                }

                Product product = new Product()
                {
                    Code = productRegistration.Code,
                    Name = productRegistration.Name,
                    Type = productRegistration.Type,
                    Description = productRegistration.Description,
                };

                if (productRegistration.Photo != null)
                {
                    product.Image = product.Code + productRegistration.Photo.FileName.Substring(productRegistration.Photo.FileName.Length - 4, 4);

                    string uploadsFolder = Path.Combine(_environment.ContentRootPath, "wwwroot/Images");
                    string filePath = Path.Combine(uploadsFolder, product.Image);

                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await productRegistration.Photo.CopyToAsync(stream);
                    }
                }

                await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();
                return new HttpBaseResult()
                {
                    Code = HttpStatusCode.OK,
                };

            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<HttpBaseResult> UpdateAsync(ProductRegistrationRequest productRegistrationRequest)
        {
           var product = await _dbContext.Products.Where(a => a.Code == productRegistrationRequest.Code).FirstOrDefaultAsync();
           if (product != null)
           {
               product.Name = productRegistrationRequest.Name;
               product.Description = productRegistrationRequest.Description;
               product.Type = productRegistrationRequest.Type;

               _dbContext.Products.Update(product);
               await _dbContext.SaveChangesAsync();
               return new HttpBaseResult()
               {
                   Success = true,
                   Code = HttpStatusCode.Created,
               };
           }

           return new HttpBaseResult()
           {
               Success = false,
               Code = HttpStatusCode.NotFound,
               Errors = new[] {"Not found is product"}
           };
        }

        public IEnumerable<Product> GetProductsSlimmedAsync()
        {
            return _dbContext.Products.AsEnumerable().Select(a => new Product()
            {
                Code = a.Code,
                Name = a.Name,
            });
        }

        public async Task<HttpBaseResult> DeleteProductAsync(string code)
        {
            var result = await _dbContext.Products.Where(a => a.Code == code).FirstOrDefaultAsync();

            if (result != null)
            {
                _dbContext.Products.Remove(result);
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

        public async Task<Product> GetProductAsync(string code)
        {
            return await _dbContext.Products.Where(a => a.Code == code).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }
    }
}