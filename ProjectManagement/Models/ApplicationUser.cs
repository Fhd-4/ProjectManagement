using Microsoft.AspNetCore.Identity;

namespace ProjectManagement.Models
{
    // This connects your project to Mohamed's Identity structure
    public class ApplicationUser : IdentityUser
    {
        // If your team wants to link a User to an Employee, they might add this line later:
        // public int? EmployeeId { get; set; }
        // public Employee Employee { get; set; }
    }
}
