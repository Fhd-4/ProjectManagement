using Microsoft.AspNetCore.Identity;

namespace ProjectManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfilePhoto { get; set; }

        public string? BackgroundPhoto { get; set; }

        public string? Name { get; set; }

        public string? Title { get; set; }

        public string? Company { get; set; }

        public string? About { get; set; }

        public string? Location { get; set; }

        public string? Website { get; set; }

        public string? LinkedIn { get; set; }

        public string? WhatsApp { get; set; }
    }
}