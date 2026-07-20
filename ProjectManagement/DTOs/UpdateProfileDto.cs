namespace ProjectManagement.DTOs
{
    public class UpdateProfileDto
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

        public string? Phone { get; set; }

        public string? LocationEn { get; set; }
        public string? LocationAr { get; set; }

        public string? Website { get; set; }

        public string? LinkedIn { get; set; }

        public string? WhatsApp { get; set; }

        public List<string?>? Skills { get; set; }

        public List<ExperienceDto>? Experiences { get; set; }

        public List<EducationDto>? Educations { get; set; }
    }
}