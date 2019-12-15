using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsBlog.Persistence;
using NewsBlog.Persistence.DTOs;

namespace NewsBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        // api/Account/Login
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginDto user)
        {
            if (_signInManager.IsSignedIn(User))
                await _signInManager.SignOutAsync();

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, isPersistent: false,
                    lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Ok();
                }

                ModelState.AddModelError("", "Bejelentkezés sikertelen!");
                return Unauthorized();
            }

            return Unauthorized();
        }

        [HttpPost("Signout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }
    }
}