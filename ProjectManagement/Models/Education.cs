namespace ProjectManagement.Models
{
    public class Education
    {
        public int Id { get; set; }
        public string? Degree { get; set; }
        public string? Field { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}