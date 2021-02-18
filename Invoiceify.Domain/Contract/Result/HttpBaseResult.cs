using System.Collections.Generic;
using System.Net;

namespace Invoiceify.Domain.Contract.Result
{
    public class HttpBaseResult
    {
        public bool Success { get; set; }
        public HttpStatusCode Code { get; set; }
        public IEnumerable<string> Errors { get; set; }   
    }
}