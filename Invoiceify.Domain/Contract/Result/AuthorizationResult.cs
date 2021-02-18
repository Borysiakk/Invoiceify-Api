using System.Collections.Generic;

namespace Invoiceify.Domain.Contract.Result
{
    public class AuthorizationResult
    {
        public string Id { get; set; }
        public string User { get; set; }
        public string Token { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }    
    }
}