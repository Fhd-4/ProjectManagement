namespace ProjectManagement.Models
{
    public class Experience
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Company { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}