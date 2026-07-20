using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ImageUploaderController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public ImageUploaderController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // POST: api/ImageUploader
        [HttpPost]
        public async Task<IActionResult> Upload( IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("يرجى اختيار صورة للرفع!");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("صيغة الملف غير مدعومة! يرجى رفع صورة بصيغة (jpg, jpeg, png, gif, webp, svg)");
            }

            var webRootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolder = Path.Combine(webRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fullUrl = $"https://localhost:44367=/uploads/{uniqueFileName}";

            return Ok(new
            {
                Message = "تم رفع الصورة بنجاح!",
                Url = fullUrl,
                FilePath = fullUrl,
                FileName = uniqueFileName
            });
        }
    }
}
