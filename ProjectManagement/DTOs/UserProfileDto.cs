namespace ProjectManagement.DTOs
{
    public class UserProfileDto
    {
        public string? ProfilePhoto { get; set; }
        public string? BackgroundPhoto { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Company { get; set; }
        public string? About { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Location { get; set; }
        public string? Website { get; set; }
        public string? LinkedIn { get; set; }
        public string? WhatsApp { get; set; }

        // التعديل: حددنا الأنواع بشكل صريح وخلينا الـ string يقبل null عشان يطابق المودل
        public List<string?>? Skills { get; set; }
        public List<ExperienceDto>? Experiences { get; set; }
        public List<EducationDto>? Educations { get; set; }
    }

    // كلاسات فرعية صغيرة داخل نفس الملف عشان ننقل بيانات الخبرة والتعليم بدون مشاكل الـ object
    public class ExperienceDto
    {
        public string? Title { get; set; }
        public string? Company { get; set; }
    }

    public class EducationDto
    {
        public string? Degree { get; set; }
        public string? Field { get; set; }
    }
}