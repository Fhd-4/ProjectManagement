using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                UserName = model.Username,
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

        // 3. ⭐️ الـ API الجديدة: جلب كل المستخدمين المسجلين في النظام
        // الرابط حقها بيكون: GET api/Auth/all-users
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            // بنسحب اليوزرز وناخذ فقط (الـ ID، الإيميل، والـ Username) عشان الحماية وما نرسل الـ Password Hash
            var users = await _userManager.Users
                .Select(u => new
                {
                    u.UserName,
                    u.Email
                })
                .ToListAsync();

            return Ok(users);
        }

        // 4. الـ API الجديدة: تغيير الرقم السري للمستخدم
        // الرابط حقها بيكون: POST api/Auth/change-password
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            // 1. البحث عن المستخدم عن طريق الإيميل
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("المستخدم غير موجود!");
            }

            // 2. تغيير الباسورد (الـ UserManager يتكفل بالتأكد من الباسورد القديمة وتشفير الجديدة تلقائياً)
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                // إذا الباسورد القديمة غلط أو الجديدة ما طابقت شروط القوة (أرقام وحروف) بيرجع الأخطاء هنا
                return BadRequest(result.Errors);
            }

            return Ok("تم تغيير الرقم السري بنجاح!");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            // تأكد إنها تقرأ model.Email 
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("المستخدم غير موجود!");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Ok(new
            {
                Message = "تم توليد رمز إعادة تعيين كلمة المرور بنجاح!",
                Token = token
            });
        }


    }
}

    
