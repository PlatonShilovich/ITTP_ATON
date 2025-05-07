using System.ComponentModel.DataAnnotations;

namespace WebApi_ITTP_ATON.Models
{
    public class User
    {
        [Required(ErrorMessage = "Guid is required")]
        public Guid Guid { get; set; }

        [Required(ErrorMessage = "Login is required")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Login must contain only Latin letters and numbers")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Password must contain only Latin letters and numbers")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯЁё]+$", ErrorMessage = "Name must contain only Latin or Russian letters")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 2, ErrorMessage = "Gender must be 0 (female), 1 (male), or 2 (unknown)")]
        public int Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public bool Admin { get; set; }

        [Required(ErrorMessage = "CreatedOn is required")]
        public DateTime CreatedOn { get; set; }

        [Required(ErrorMessage = "CreatedBy is required")]
        public string CreatedBy { get; set; } = string.Empty;

        [Required(ErrorMessage = "ModifiedOn is required")]
        public DateTime ModifiedOn { get; set; }

        [Required(ErrorMessage = "ModifiedBy is required")]
        public string ModifiedBy { get; set; } = string.Empty;

        [Required(ErrorMessage = "RevokedOn is required")]
        public DateTime RevokedOn { get; set; }

        [Required(ErrorMessage = "RevokedBy is required")]
        public string RevokedBy { get; set; } = string.Empty;
    }
}