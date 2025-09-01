using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.ClassDto
{
    public class UserDto
    {
        [Required(ErrorMessage = "Email est requis.")]
        [EmailAddress(ErrorMessage = "Email invalide.")]
        [StringLength(254, ErrorMessage = "Email trop long.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password est requis.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password doit contenir au moins 6 caractères.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Fullname est requis.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Fullname doit contenir entre 3 et 100 caractères.")]
        public string Fullname { get; set; }
    }
}
