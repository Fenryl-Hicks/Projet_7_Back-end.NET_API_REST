using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.ClassDto
{
    public class RatingDto
    {
        [StringLength(20, ErrorMessage = "Moody's rating trop long.")]
        public string MoodysRating { get; set; }

        [StringLength(20, ErrorMessage = "S&P rating trop long.")]
        public string SandPRating { get; set; }

        [StringLength(20, ErrorMessage = "Fitch rating trop long.")]
        public string FitchRating { get; set; }

        [Required(ErrorMessage = "OrderNumber est requis.")]
        [Range(1, 300, ErrorMessage = "OrderNumber doit être compris entre 1 et 300.")]
        public byte? OrderNumber { get; set; }
    }
}
