using System.ComponentModel.DataAnnotations;

namespace WebApi_ITTP_ATON.Models
{
    public class UpdatePasswordRequestDTO
    {
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Password must contain only Latin letters and numbers")]
        public string Password { get; set; } = string.Empty;
    }
}