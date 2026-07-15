using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace ProjectManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

    {
     
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
    }
}