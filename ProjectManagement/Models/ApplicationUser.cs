using Microsoft.AspNetCore.Identity;

namespace ProjectManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ResetOtp { get; set; }

        public DateTime? ResetOtpExpiry { get; set; }
    }
}