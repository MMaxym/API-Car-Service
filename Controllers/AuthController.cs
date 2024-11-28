using System.Security.Claims;
using API.Models;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
    // [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly AuthService _authService;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, AuthService authService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    return Unauthorized();
                }

                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "User";
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, role),
                    new Claim("UserId", user.Id.ToString()),
                };
                
                var token = await _authService.GenerateJwtTokenAsync(user, role, claims);
                
                return Ok(new { Token = token, Role = role, UserId = user.Id, Name = user.Name, Surname = user.Surname, Email = user.Email, Phone = user.PhoneNumber });
            }

            return Unauthorized();
        }


        [HttpPost ("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }