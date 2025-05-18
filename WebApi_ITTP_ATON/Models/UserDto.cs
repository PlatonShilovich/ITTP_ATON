using System.ComponentModel.DataAnnotations;

namespace WebApi_ITTP_ATON.Models
{
    public class UserDto
    {
        [Required(ErrorMessage = "Guid обязателен")]
        public Guid Guid { get; set; }

        [Required(ErrorMessage = "Логин обязателен")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Логин должен содержать только латинские буквы и цифры")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Пароль должен содержать только латинские буквы и цифры")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имя обязательно")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯЁё]+$", ErrorMessage = "Имя должно содержать только латинские или русские буквы")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 2, ErrorMessage = "Пол должен быть 0 (женский), 1 (мужской) или 2 (неизвестно)")]
        public int Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public bool Admin { get; set; }

        [Required(ErrorMessage = "Дата создания обязательна")]
        public DateTime CreatedOn { get; set; }

        [Required(ErrorMessage = "Кем создано обязательно")]
        public string CreatedBy { get; set; } = string.Empty;

        [Required(ErrorMessage = "Дата изменения обязательна")]
        public DateTime ModifiedOn { get; set; }

        [Required(ErrorMessage = "Кем изменено обязательно")]
        public string ModifiedBy { get; set; } = string.Empty;

        public DateTime? RevokedOn { get; set; }

        public string? RevokedBy { get; set; }
    }
}
