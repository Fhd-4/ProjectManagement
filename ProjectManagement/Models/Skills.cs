namespace ProjectManagement.Models
{
    public class Skills
    {
        public int Id { get; set; }
        public string? Skill { get; set; } 
      
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}