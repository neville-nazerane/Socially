using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socially.Core.Entities;
using Socially.Core.Models;
using Socially.Server.Managers;
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
        private readonly SignInManager<User> _signInManager;
        private readonly IUserVerificationManager _userVerificationManager;

        public AccountController(SignInManager<User> signInManager, IUserVerificationManager userVerificationManager)
        {
            _signInManager = signInManager;
            _userVerificationManager = userVerificationManager;
        }

        [HttpGet("verifyEmail/{email}")]
        public Task<bool> VerifyEmail(string email, CancellationToken cancellationToken = default)
            => _userVerificationManager.EmailExistsAsync(email, cancellationToken);

        [HttpGet("verifyUsername/{userName}")]
        public Task<bool> VerifyUsername(string userName, CancellationToken cancellationToken = default) 
            => _userVerificationManager.UserNameExistsAsync(userName, cancellationToken);

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            var result = await _signInManager.UserManager.CreateAsync(new User { 
                                    Email = model.Email,
                                    UserName = model.UserName
                                }, model.Password);
            if (result.Succeeded) return Ok();
            else return BadRequest(new Dictionary<string, IEnumerable<string>> {
                { string.Empty, result.Errors.Select(r => r.Description) }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Succeeded) return Ok();
            else return BadRequest();
        }

    }
}
