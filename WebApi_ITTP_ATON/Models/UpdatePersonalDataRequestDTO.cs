using System.ComponentModel.DataAnnotations;

namespace WebApi_ITTP_ATON.Models
{
    public class UpdatePersonalDataRequestDTO
    {
        [RegularExpression(@"^[a-zA-Zа-яА-ЯЁё]+$", ErrorMessage = "Name must contain only Latin or Russian letters")]
        public string? Name { get; set; }

        [Range(0, 2, ErrorMessage = "Gender must be 0 (female), 1 (male), or 2 (unknown)")]
        public int? Gender { get; set; }

        public DateTime? Birthday { get; set; }
    }
}