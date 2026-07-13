using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models;

namespace ProjectManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        // هذا الكونستركتور يمرر إعدادات الاتصال لقاعدة البيانات
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // هنا نقول له كرت لنا جدول اسمه Employees بناءً على كلاس الـ Employee
        public DbSet<Employee> Employees { get; set; }
    }
}