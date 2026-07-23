using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Data;
using ProjectManagement.DTOs;
using ProjectManagement.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        // Hi am yusra
        // 1. رابط تسجيل حساب جديد: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return BadRequest("هذا البريد الإلكتروني مسجل مسبقاً!");

            var user = new ApplicationUser
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
                var roles = await _userManager.GetRolesAsync(user);
                // توليد كرت الأمان (Token) للمستخدم
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var role in roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

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
                    id = user.Id,
                    userId = user.Id,
                    username = user.UserName,
                    email = user.Email,
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,

                    user = new
                    {
                        id = user.Id,
                        username = user.UserName,
                        email = user.Email,

                        nameEn = user.NameEn,
                        nameAr = user.NameAr,

                        titleEn = user.TitleEn,
                        titleAr = user.TitleAr,

                        companyEn = user.CompanyEn,
                        companyAr = user.CompanyAr,

                        profilePhoto = user.ProfilePhoto,
                        backgroundPhoto = user.BackgroundPhoto,

                        locationEn = user.LocationEn,
                        locationAr = user.LocationAr,

                        website = user.Website,
                        linkedIn = user.LinkedIn,
                        whatsApp = user.WhatsApp,

                        roles = roles
                    }
                });
            }

            return Unauthorized("الإيميل أو الرقم السري غير صحيح!");
        }
        
        [HttpPost("create-client")]
        public async Task<IActionResult> CreateClient(CreateClientDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                return BadRequest("Email already exists.");
            }

            DateTime expirationDate = model.Unit switch
            {
                "Minute" => DateTime.UtcNow.AddMinutes(model.Duration),
                "Hour" => DateTime.UtcNow.AddHours(model.Duration),
                "Day" => DateTime.UtcNow.AddDays(model.Duration),
                "Month" => DateTime.UtcNow.AddMonths(model.Duration),
                "Year" => DateTime.UtcNow.AddYears(model.Duration),
                _ => DateTime.UtcNow
            };

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,

                NameEn = model.ClientName,

                Duration = model.Duration,
                Unit = model.Unit,

                SubscriptionStartDate = DateTime.UtcNow,
                SubscriptionEndDate = expirationDate,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (!await _roleManager.RoleExistsAsync("Client"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Client"));
            }

            await _userManager.AddToRoleAsync(user, "Client");

            return Ok(new
            {
                Message = "Client created successfully."
            });
        }
        // 3. ⭐️ الـ API الجديدة: جلب كل المستخدمين المسجلين في النظام
        // الرابط حقها بيكون: GET api/Auth/all-users
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users
                .Select(u => new
                {
                    u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    ClientName = u.NameEn,
                    u.Duration,
                    u.Unit,
                    u.SubscriptionStartDate,
                    u.SubscriptionEndDate,
                    Avatar = u.ProfilePhoto,
                    Role = _context.UserRoles
                        .Where(ur => ur.UserId == u.Id)
                        .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                        .FirstOrDefault() ?? "Client"
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

            // توليد رمز مكون من 6 أرقام (OTP) باستخدام الـ Email token provider
            var token = await _userManager.GenerateUserTokenAsync(user, "Email", "ResetPassword");

            return Ok(new
            {
                Message = "تم توليد رمز إعادة تعيين كلمة المرور بنجاح!",
                Token = token
            });
        }

        // 6. الـ API الخاصة بإعادة تعيين كلمة المرور (Reset Password)
        // POST api/Auth/reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            // البحث عن المستخدم بالإيميل
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("المستخدم غير موجود!");
            }

            // التحقق من الرمز المكون من 6 أرقام
            var isValid = await _userManager.VerifyUserTokenAsync(user, "Email", "ResetPassword", model.Token);
            if (!isValid)
            {
                return BadRequest(new[] { new { code = "InvalidToken", description = "رمز إعادة التعيين غير صحيح أو انتهت صلاحيته!" } });
            }

            // توليد توكن إعادة تعيين كلمة المرور الفعلي داخلياً لإتمام العملية بـ ResetPasswordAsync
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("تم إعادة تعيين كلمة المرور الجديدة بنجاح!");
        }
        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto model)
        {
            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);

            if (roleExists)
            {
                return BadRequest("Role already exists");
            }

            var role = new IdentityRole(model.RoleName);

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Role created successfully");
        }
        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles
                .Select(r => new
                {
                    r.Id,
                    r.Name
                })
                .ToListAsync();

            return Ok(roles);
        }
        [HttpPost("create-superadmin")]
        public async Task<IActionResult> CreateSuperAdmin(
    [FromBody] CreateSuperAdminDto model)
        {
            // Check if SuperAdmin role exists
            var roleExists = await _roleManager.RoleExistsAsync("SuperAdmin");

            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }


            // Check if user already exists
            var userExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                return BadRequest("User already exists");
            }


            // Create the user
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                EmailConfirmed = true
            };


            var result = await _userManager.CreateAsync(user, model.Password);


            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }


            // Assign SuperAdmin role
            await _userManager.AddToRoleAsync(user, "SuperAdmin");


            return Ok("SuperAdmin created successfully");
        }

        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("المستخدم غير موجود!");
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new { Message = "تم حذف المستخدم بنجاح!" });
        }

        [HttpPut("edit-user/{userId}")]
        public async Task<IActionResult> EditUser(string userId, [FromBody] EditClientDto model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("المستخدم غير موجود!");
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null && existingUser.Id != userId)
            {
                return BadRequest("البريد الإلكتروني مستخدم بالفعل!");
            }

            user.Email = model.Email;
            user.UserName = model.Email;
            user.NameEn = model.ClientName;
            user.Duration = model.Duration;
            user.Unit = model.Unit;

            DateTime expirationDate = model.Unit switch
            {
                "Minute" => DateTime.UtcNow.AddMinutes(model.Duration),
                "Hour" => DateTime.UtcNow.AddHours(model.Duration),
                "Day" => DateTime.UtcNow.AddDays(model.Duration),
                "Month" => DateTime.UtcNow.AddMonths(model.Duration),
                "Year" => DateTime.UtcNow.AddYears(model.Duration),
                _ => DateTime.UtcNow
            };
            user.SubscriptionEndDate = expirationDate;

            if (!string.IsNullOrEmpty(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (!resetResult.Succeeded)
                {
                    return BadRequest(resetResult.Errors);
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { Message = "تم تحديث بيانات العميل بنجاح!" });
        }
    }
}

    
