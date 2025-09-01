using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.ClassDto
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email est requis.")]
        [EmailAddress(ErrorMessage = "Email invalide.")]
        [StringLength(254, ErrorMessage = "Email trop long.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password est requis.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password doit contenir au moins 8 caractères.")]
        public string Password { get; set; }
    }
}
