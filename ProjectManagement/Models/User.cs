using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string? ProfilePhoto { get; set; }

        public string? BackgroundPhoto { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Title { get; set; }

        public string? Company { get; set; }

        public string? About { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Location { get; set; }

        public string? Website { get; set; }

        public string? LinkedIn { get; set; }

        public string? WhatsApp { get; set; }
    }
}