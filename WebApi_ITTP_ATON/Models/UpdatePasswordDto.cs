using System.ComponentModel.DataAnnotations;

namespace WebApi_ITTP_ATON.Models
{
    public class UpdatePasswordDto
    {
        [Required(ErrorMessage = "Пароль обязателен")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Пароль должен содержать только латинские буквы и цифры")]
        public string Password { get; set; } = string.Empty;
    }
}
