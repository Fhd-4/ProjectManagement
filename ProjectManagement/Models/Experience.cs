namespace ProjectManagement.Models
{
    public class Experience
    {
        public int Id { get; set; }

        public string? TitleEn { get; set; }
        public string? TitleAr { get; set; }

        public string? CompanyEn { get; set; }
        public string? CompanyAr { get; set; }

        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }
    }
}