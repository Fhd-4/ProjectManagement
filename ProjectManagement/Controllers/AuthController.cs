using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // 1. رابط تسجيل حساب جديد: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return BadRequest("هذا البريد الإلكتروني مسجل مسبقاً!");

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            // الـ UserManager هنا بيتكفل بتشفير الباسورد وحفظها في جدول AspNetUsers تلقائياً
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("تم تسجيل الحساب بنجاح!");
        }

        // 2. رابط تسجيل الدخول وصناعة الـ Token: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            // التحقق من الإيميل وتطابق الباسورد المشفرة
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // توليد كرت الأمان (Token) للمستخدم
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("FahdProjectManagementSecretKey2026_JWT_Secure!"));

                var token = new JwtSecurityToken(
                    issuer: "ProjectManagementSecretIssuer",
                    audience: "ProjectManagementSecretAudience",
                    expires: DateTime.Now.AddHours(3), // الكرت صالح لمدة 3 ساعات
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized("الإيميل أو الرقم السري غير صحيح!");
        }
    }
}