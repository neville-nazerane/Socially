using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Socially.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.WebAPI.Controllers
{

    [ApiController]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

    }
}
