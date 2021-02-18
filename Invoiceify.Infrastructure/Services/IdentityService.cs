using System;
using System.Linq;
using System.Threading.Tasks;
using Invoiceify.Domain.Contract.Requests;
using Invoiceify.Domain.Contract.Result;
using Invoiceify.Domain.Entities;
using Invoiceify.Infrastructure.Interface;
using Microsoft.AspNetCore.Identity;

namespace Invoiceify.Infrastructure.Services
{
    public class IdentityService :IIdentityService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityService(ITokenService tokenService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<AuthorizationResult> LoginAsync(LoginModelView loginModelView)
        {
            
            Console.WriteLine(loginModelView.Email);
            var result = await _signInManager.PasswordSignInAsync(loginModelView.Email, loginModelView.Password, loginModelView.RememberMe,false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(loginModelView.Email);
                return new AuthorizationResult()
                {
                    Success = true,
                    User = user.Email,
                    Token = _tokenService.Generate(user),
                };
            }

            return new AuthorizationResult()
            {
                Success = false,
                Errors = new[] {"Błędny login lub hasło"}
            };
        }

        public async Task<AuthorizationResult> RegisterAsync(RegisterViewModel registerViewModel)
        {
            var user = new ApplicationUser() {Email = registerViewModel.Email, UserName = registerViewModel.Email};
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return new AuthorizationResult()
                {
                    Id = user.Id,
                    Success = true,
                    Token = _tokenService.Generate(user),
                };
            }
            
            return new AuthorizationResult()
            {
                Success = false,
                Errors = result.Errors.Select(a => a.Description).ToArray(),
            };
        }
    }
}