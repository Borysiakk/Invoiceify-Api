using Microsoft.AspNetCore.Http;

namespace Invoiceify.Domain.Contract.Requests
{
    public class ProductRegistrationRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IFormFile Photo { get; set; }
        public string Type { get; set; }
    }
}