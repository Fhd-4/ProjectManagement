using Microsoft.AspNetCore.Identity;

namespace ProjectManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfilePhoto { get; set; }

        public string? BackgroundPhoto { get; set; }

        public string? NameEn { get; set; }
        public string? NameAr { get; set; }

        public string? TitleEn { get; set; }
        public string? TitleAr { get; set; }

        public string? CompanyEn { get; set; }
        public string? CompanyAr { get; set; }

        public string? AboutEn { get; set; }
        public string? AboutAr { get; set; }

        public string? LocationEn { get; set; }
        public string? LocationAr { get; set; }

        public string? Website { get; set; }

        public string? LinkedIn { get; set; }

        public string? WhatsApp { get; set; }

        public ICollection<Skills> Skills { get; set; } = new List<Skills>();

        public ICollection<Experience> Experiences { get; set; } = new List<Experience>();

        public ICollection<Education> Educations { get; set; } = new List<Education>();
    }
}