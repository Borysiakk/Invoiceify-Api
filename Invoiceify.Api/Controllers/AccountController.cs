using System;
using System.Threading.Tasks;
using Invoiceify.Domain.Contract.Requests;
using Invoiceify.Domain.Entities;
using Invoiceify.Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Invoiceify.Api.Controllers
{
    [ApiController]
    [Route("api/Account")]
    public class AccountController :ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginModelView login)
        {
            try
            {
                var result = await _identityService.LoginAsync(login);
                if (result.Success)
                {
                    return Ok(result);
                }

                return StatusCode(StatusCodes.Status401Unauthorized,result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,e.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            try
            {
                var result = await _identityService.RegisterAsync(register);
                if (result.Success)
                {
                    return Ok(result);
                }

                return new BadRequestObjectResult(result.Errors);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,e.Message);
            }
        }
    }
}