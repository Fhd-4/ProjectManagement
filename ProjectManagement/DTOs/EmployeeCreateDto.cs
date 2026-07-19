using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.DTOs
{
    public class EmployeeCreateDto
    {
        [Required(ErrorMessage = "الاسم مطلوب ولا يمكن تركه فارغاً")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "الاسم يجب أن يكون بين 3 إلى 100 حرف")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "العنوان مطلوب")]
        public string Address { get; set; } = string.Empty;
 
        [Range(18, 60, ErrorMessage = "العمر يجب أن يكون بين 18 و 60 سنة")]
        public int Age { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "صيغة رقم الهاتف غير صحيحة")]
        public string Phone { get; set; } = string.Empty;
    }
}