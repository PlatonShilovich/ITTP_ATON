using System.ComponentModel.DataAnnotations;

namespace WebApi_ITTP_ATON.Models
{
    public class AddUserDto
    {
        [Required(ErrorMessage = "Логин обязателен")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Логин должен содержать только латинские буквы и цифры")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Пароль должен содержать только латинские буквы и цифры")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имя обязателен")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯЁё]+$", ErrorMessage = "Имя должно содержать только латинские или русские буквы")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пол обязателен")]
        [Range(0, 2, ErrorMessage = "Пол должен быть 0 (женский), 1 (мужской), или 2 (неизвестен)")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Роль обязательна")]
        public bool Admin { get; set; }
    }
}
