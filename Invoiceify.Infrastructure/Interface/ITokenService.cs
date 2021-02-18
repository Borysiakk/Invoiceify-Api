using Invoiceify.Domain.Entities;

namespace Invoiceify.Infrastructure.Interface
{
    public interface ITokenService
    {
        public string Generate(ApplicationUser user);
    }
}