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
        [Authorize]
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
        [Authorize]
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
            user.NameEn = model.NameEn;
            user.NameAr = model.NameAr;
            user.TitleEn = model.TitleEn;
            user.TitleAr = model.TitleAr;
            user.CompanyEn = model.CompanyEn;
            user.CompanyAr = model.CompanyAr;
            user.AboutEn = model.AboutEn;
            user.AboutAr = model.AboutAr;
            user.LocationEn = model.LocationEn;
            user.LocationAr = model.LocationAr;

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
                        _context.Experiences.Add(new Experience
                        {
                            TitleEn = exp.TitleEn,
                            TitleAr = exp.TitleAr,

                            CompanyEn = exp.CompanyEn,
                            CompanyAr = exp.CompanyAr,

                            UserId = user.Id
                        });
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
                        _context.Educations.Add(new Education
                        {
                            DegreeEn = edu.DegreeEn,
                            DegreeAr = edu.DegreeAr,

                            FieldEn = edu.FieldEn,
                            FieldAr = edu.FieldAr,

                            UserId = user.Id
                        });
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

        // GET: api/Profile/preview/{userId}  أو  GET: api/Profile/{userId} (معاينة كرت العميل العامة - حق زر العين)
        [AllowAnonymous]
        [HttpGet("{userId}")]
        [HttpGet("preview/{userId}")]
        [HttpGet("public/{userId}")]
        public async Task<IActionResult> GetUserProfileById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
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
                .Select(e => new ExperienceDto {
                    TitleEn = e.TitleEn,TitleAr = e.TitleAr,
                    CompanyEn = e.CompanyEn,CompanyAr = e.CompanyAr})

                .ToListAsync();

            var educations = await _context.Educations
                .Where(e => e.UserId == user.Id)
                .Select(e => new EducationDto {
                    DegreeEn = e.DegreeEn,
                    DegreeAr = e.DegreeAr,

                    FieldEn = e.FieldEn,
                    FieldAr = e.FieldAr
                })
                .ToListAsync();

            return new UserProfileDto
            {
                Id = user.Id,
                Username = user.UserName,
                ProfilePhoto = user.ProfilePhoto,
                BackgroundPhoto = user.BackgroundPhoto,
                NameEn = user.NameEn,
                NameAr = user.NameAr,

                TitleEn = user.TitleEn,
                TitleAr = user.TitleAr,

                CompanyEn = user.CompanyEn,
                CompanyAr = user.CompanyAr,

                AboutEn = user.AboutEn,
                AboutAr = user.AboutAr,

                LocationEn = user.LocationEn,
                LocationAr = user.LocationAr,
                Phone = user.PhoneNumber,
                Email = user.Email,
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
