namespace ProjectManagement.DTOs
{
    public class UserProfileDto
    {
        public string? Id { get; set; }

        public string? Username { get; set; }

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

        public string? Email { get; set; }

        public string? LocationEn { get; set; }
        public string? LocationAr { get; set; }

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
        public string? TitleEn { get; set; }
        public string? TitleAr { get; set; }

        public string? CompanyEn { get; set; }
        public string? CompanyAr { get; set; }
    }

    public class EducationDto
    {
        public string? DegreeEn { get; set; }
        public string? DegreeAr { get; set; }

        public string? FieldEn { get; set; }
        public string? FieldAr { get; set; }
    }
}