using System.ComponentModel.DataAnnotations;

namespace WebApi_ITTP_ATON.Models
{
    public class UpdateLoginDto
    {
        [Required(ErrorMessage = "Логин обязателен")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Логин должен содержать только латинские буквы и цифры")]
        public string Login { get; set; } = string.Empty;
    }
}
