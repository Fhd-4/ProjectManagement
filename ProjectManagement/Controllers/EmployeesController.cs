using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.DTOs;
using ProjectManagement.Models;

namespace ProjectManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // هنا نحقن قاعدة البيانات داخل الكونترولر عشان نقدر نستخدمها
        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // test
        // 1. الـ EndPoint الأول: لقراءة وعرض كل الموظفين (GET)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }

        // دالة عرض بيانات موظف واحد فقط باستخدام الـ ID (GET by ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            // إذا الموظف مو موجود في قاعدة البيانات نرجع خطأ 404
            if (employee == null)
            {
                return NotFound(new { message = $"الموظف صاحب الرقم {id} غير موجود" });
            }

            return Ok(employee);
        }

        // 2. الـ EndPoint لإنشاء موظف جديد (POST) - بدون إظهار الـ Id في سواجر
        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(EmployeeCreateDto dto)
        {
            // ننقل البيانات من الـ DTO إلى كلاس الـ Employee الأساسي حق الداتابيس
            var employee = new Employee
            {
                Name = dto.Name,
                Address = dto.Address,
                Age = dto.Age,
                Phone = dto.Phone
                // الـ Id والـ CreatedAt بيتحملوا تلقائياً
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }
        // 3. الـ EndPoint الثالث: تعديل بيانات موظف معين (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest("رقم الموظف غير متطابق مع الرابط");
            }

            // نبلغ الـ Entity Framework إن البيانات هذي تم تعديلها
            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // نتأكد إذا كان الموظف موجود أصلاً في الداتابيس أو انحذف أثناء التعديل
                if (!_context.Employees.Any(e => e.Id == id))
                {
                    return NotFound("الموظف غير موجود");
                }
                throw;
            }

            return NoContent(); // تعني تمت العملية بنجاح وبدون إرجاع محتوى
        }

        // 4. الـ EndPoint الرابع: مسح موظف معين (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound("الموظف غير موجود في قاعدة البيانات");
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"تم حذف الموظف {employee.Name} بنجاح" });
        }

    }


}