using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ProjectManagement.Models;
using ProjectManagement.DTOs;

namespace ProjectManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // 1. Register Endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully!" });
            }

            return BadRequest(result.Errors);
        }

        // 2. Login Endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                return Ok(new { Message = "Logged in successfully!" });
            }

            return Unauthorized(new { Message = "Invalid email or password" });
        }

        // 3. Get All Users Endpoint
        // test
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.Select(u => new
            {
                u.Id,
                u.Email,
                u.UserName
            }).ToList();

            return Ok(users);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound(new { Message = "المستخدم غير موجود!" });
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { Message = "تم تغيير كلمة المرور بنجاح!" });
            }
            return BadRequest(result.Errors);
        }

    }
}
