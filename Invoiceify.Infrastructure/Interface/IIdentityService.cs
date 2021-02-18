
using System.Net;
using System.Threading.Tasks;
using Invoiceify.Domain.Contract.Requests;
using Invoiceify.Domain.Contract.Result;

namespace Invoiceify.Infrastructure.Interface
{
    public interface IIdentityService
    {
        public Task<AuthorizationResult> LoginAsync(LoginModelView loginModelView);
        public Task<AuthorizationResult> RegisterAsync(RegisterViewModel registerViewModel);
    }
}