using System.ComponentModel.DataAnnotations;

namespace WebApi_ITTP_ATON.Models
{
    public class UpdatePersonalDataDto
    {
        [RegularExpression(@"^[a-zA-Zа-яА-ЯЁё]+$", ErrorMessage = "Имя должно содержать только латинские или русские буквы")]
        public string? Name { get; set; }

        [Range(0, 2, ErrorMessage = "Пол должен быть 0 (женский), 1 (мужской), или 2 (неизвестный)")]
        public int? Gender { get; set; }

        public DateTime? Birthday { get; set; }
    }
}
