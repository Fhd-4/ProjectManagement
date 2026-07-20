namespace ProjectManagement.Models
{
    public class Education
    {
        public int Id { get; set; }

        public string? DegreeEn { get; set; }
        public string? DegreeAr { get; set; }

        public string? FieldEn { get; set; }
        public string? FieldAr { get; set; }

        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }
    }
}