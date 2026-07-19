namespace ProjectManagement.DTOs
{
    public class UpdateProfileDto
    {
        public string? ProfilePhoto { get; set; }

        public string? BackgroundPhoto { get; set; }

        public string? Name { get; set; }

        public string? Title { get; set; }

        public string? Company { get; set; }

        public string? About { get; set; }

        public string? Phone { get; set; }

        public string? Location { get; set; }

        public string? Website { get; set; }

        public string? LinkedIn { get; set; }

        public string? WhatsApp { get; set; }

        public List<string?>? Skills { get; set; }

        public List<ExperienceDto>? Experiences { get; set; }

        public List<EducationDto>? Educations { get; set; }
    }
}