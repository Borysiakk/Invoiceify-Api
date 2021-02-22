using System;
using System.Net;
using System.Threading.Tasks;
using Invoiceify.Domain.Contract.Requests;
using Invoiceify.Domain.Entities;
using Invoiceify.Infrastructure.Interface;
using Invoiceify.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Invoiceify.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Products")]
    public class ProductsController: ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm]ProductRegistrationRequest productRegistrationRequest)
        {
            try
            {
                var result = await _productRepository.AddAsync(productRegistrationRequest);
                if (result.Code == HttpStatusCode.Conflict)
                {
                    return NotFound($"Istnieje już produkt o tym kodzie"); 
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
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            try
            {
                Console.WriteLine("Code :{0})",id);
                var product = await _productRepository.GetProductAsync(id);
                return product;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                //await Task.Delay(5000);
                return Ok(await _productRepository.GetProductsAsync());
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
                var result  = await _productRepository.DeleteProductAsync(id);
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

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductRegistrationRequest product)
        {
            try
            {
                var result  = await _productRepository.UpdateAsync(product);
                if (result.Code == HttpStatusCode.NotFound)
                {
                    return NotFound($"Nie znaleziono produktu o kodzie : {product.Code}");
                }

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpGet("GetProductsSlimmed")]
        public IActionResult GetProductsSlimmed()
        {
            try
            {
                return Ok(_productRepository.GetProductsSlimmedAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}