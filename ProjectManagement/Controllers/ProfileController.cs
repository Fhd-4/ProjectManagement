using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.DTOs;
using ProjectManagement.Models;
using System.Security.Claims;

namespace ProjectManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: api/Profile/me  أو  GET: api/Profile
        [HttpGet("me")]
        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("لم يتم التعرف على المستخدم الحالي!");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("المستخدم غير موجود!");
            }

            var profileDto = await BuildUserProfileDtoAsync(user);
            return Ok(profileDto);
        }

        // PUT/POST: api/Profile/me  أو  api/Profile (حفظ وإنشاء بيانات الكرت بالكامل للمستخدم)
        [HttpPut("me")]
        [HttpPost("me")]
        [HttpPut]
        [HttpPost]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("لم يتم التعرف على المستخدم الحالي!");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("المستخدم غير موجود!");
            }

            // تحديث البيانات الشخصية والصورة الشخصية والغلاف وحسابات التواصل
            user.ProfilePhoto = model.ProfilePhoto;
            user.BackgroundPhoto = model.BackgroundPhoto;
            user.Name = model.Name;
            user.Title = model.Title;
            user.Company = model.Company;
            user.About = model.About;
            user.PhoneNumber = model.Phone;
            user.Location = model.Location;
            user.Website = model.Website;
            user.LinkedIn = model.LinkedIn;
            user.WhatsApp = model.WhatsApp;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // تحديث المهارات (Skills)
            if (model.Skills != null)
            {
                var existingSkills = await _context.Skills.Where(s => s.UserId == user.Id).ToListAsync();
                _context.Skills.RemoveRange(existingSkills);

                foreach (var skillName in model.Skills)
                {
                    if (!string.IsNullOrWhiteSpace(skillName))
                    {
                        _context.Skills.Add(new Skills { Skill = skillName, UserId = user.Id });
                    }
                }
            }

            // تحديث الخبرات (Experiences)
            if (model.Experiences != null)
            {
                var existingExperiences = await _context.Experiences.Where(e => e.UserId == user.Id).ToListAsync();
                _context.Experiences.RemoveRange(existingExperiences);

                foreach (var exp in model.Experiences)
                {
                    if (exp != null)
                    {
                        _context.Experiences.Add(new Experience { Title = exp.Title, Company = exp.Company, UserId = user.Id });
                    }
                }
            }

            // تحديث التعليم (Educations)
            if (model.Educations != null)
            {
                var existingEducations = await _context.Educations.Where(e => e.UserId == user.Id).ToListAsync();
                _context.Educations.RemoveRange(existingEducations);

                foreach (var edu in model.Educations)
                {
                    if (edu != null)
                    {
                        _context.Educations.Add(new Education { Degree = edu.Degree, Field = edu.Field, UserId = user.Id });
                    }
                }
            }

            await _context.SaveChangesAsync();

            var updatedProfile = await BuildUserProfileDtoAsync(user);
            return Ok(new
            {
                Message = "تم حفظ وتحديث كرت البيانات بنجاح!",
                Profile = updatedProfile
            });
        }

        // GET: api/Profile/preview/{userId}  أو  GET: api/Profile/{userId} (معاينة كرت العميل العامة - حق زرر العين)
        [AllowAnonymous]
        [HttpGet("{userId}")]
        [HttpGet("preview/{userId}")]
        [HttpGet("public/{userId}")]
        public async Task<IActionResult> GetUserProfileById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // إذا لم يكن ID قد يكون اسم المستخدم UserName
                user = await _userManager.FindByNameAsync(userId);
            }

            if (user == null)
            {
                return NotFound("المستخدم غير موجود!");
            }

            var profileDto = await BuildUserProfileDtoAsync(user);
            return Ok(profileDto);
        }

        private async Task<UserProfileDto> BuildUserProfileDtoAsync(ApplicationUser user)
        {
            var skills = await _context.Skills
                .Where(s => s.UserId == user.Id)
                .Select(s => s.Skill)
                .ToListAsync();

            var experiences = await _context.Experiences
                .Where(e => e.UserId == user.Id)
                .Select(e => new ExperienceDto { Title = e.Title, Company = e.Company })
                .ToListAsync();

            var educations = await _context.Educations
                .Where(e => e.UserId == user.Id)
                .Select(e => new EducationDto { Degree = e.Degree, Field = e.Field })
                .ToListAsync();

            return new UserProfileDto
            {
                Id = user.Id,
                Username = user.UserName,
                ProfilePhoto = user.ProfilePhoto,
                BackgroundPhoto = user.BackgroundPhoto,
                Name = user.Name,
                Title = user.Title,
                Company = user.Company,
                About = user.About,
                Phone = user.PhoneNumber,
                Email = user.Email,
                Location = user.Location,
                Website = user.Website,
                LinkedIn = user.LinkedIn,
                WhatsApp = user.WhatsApp,
                Skills = skills,
                Experiences = experiences,
                Educations = educations
            };
        }
    }
}
