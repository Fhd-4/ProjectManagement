using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models;

namespace ProjectManagement.Data
{
    // غيرناه هنا ليرث من IdentityDbContext
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Skills> Skill { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Education> Educations { get; set; }
        
    }
}