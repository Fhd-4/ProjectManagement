using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace ProjectManagement.Data
{
    // غيرناه هنا ليرث من IdentityDbContext
    public class ApplicationDbContext : IdentityDbContext
    {
        // هذا الكونستركتور يمرر إعدادات الاتصال لقاعدة البيانات
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // هنا نقول له كرت لنا جدول اسمه Employees بناءً على كلاس الـ Employee
        // هنا نقول له كرت لنا جدول اسمه Employees بناءً على كلاس الـ Employee
        // هنا نقول له كرت لنا جدول اسمه Employees بناءً على كلاس الـ Employee
        public DbSet<Employee> Employees { get; set; }
    }
}