namespace ProjectManagement.DTOs
{
    public class CreateClientDto
    {
        public string ClientName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int Duration { get; set; }

        public string Unit { get; set; } = string.Empty;
    }
}