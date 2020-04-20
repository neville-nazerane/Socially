using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socially.Core.Entities;
using Socially.Core.Models;
using Socially.Server.Managers;
using Socially.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Controllers
{

    [ApiController, Route("api/[Controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("verifyEmail/{email}")]
        public Task<bool> VerifyEmail(string email, CancellationToken cancellationToken = default)
            => _userService.VerifyEmailAsync(email, cancellationToken);

        [HttpGet("verifyUsername/{userName}")]
        public Task<bool> VerifyUsername(string userName, CancellationToken cancellationToken = default) 
            => _userService.VerifyUsernameAsync(userName, cancellationToken);

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var result = await _userService.SignUpAsync(model);
            if (result.Succeeded) return Ok();
            else return BadRequest(new Dictionary<string, IEnumerable<string>> {
                { string.Empty, result.Errors.Select(r => r.Description) }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await _userService.LoginAsync(model);
            if (result.Succeeded) return Ok();
            else return BadRequest();
        }

    }
}
