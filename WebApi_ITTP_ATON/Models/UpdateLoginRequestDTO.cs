using System.ComponentModel.DataAnnotations;

namespace WebApi_ITTP_ATON.Models
{
    public class UpdateLoginRequestDTO
    {
        [Required(ErrorMessage = "Login is required")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Login must contain only Latin letters and numbers")]
        public string Login { get; set; } = string.Empty;
    }
}