using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/Profile/me
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

            var profileDto = new UserProfileDto
            {
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
                WhatsApp = user.WhatsApp
            };

            return Ok(profileDto);
        }

        // PUT: api/Profile/me
        [HttpPut("me")]
        [HttpPut]
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

            return Ok(new
            {
                Message = "تم تحديث الملف الشخصي بنجاح!",
                Profile = new UserProfileDto
                {
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
                    WhatsApp = user.WhatsApp
                }
            });
        }

        // GET: api/Profile/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfileById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("المستخدم غير موجود!");
            }

            var profileDto = new UserProfileDto
            {
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
                WhatsApp = user.WhatsApp
            };

            return Ok(profileDto);
        }
    }
}
