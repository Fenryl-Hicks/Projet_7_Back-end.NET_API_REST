using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Dtos.Ratings;

public class UpdateRatingRequestDto
{
    [StringLength(20)] public string? MoodysRating { get; set; }
    [StringLength(20)] public string? SandPRating { get; set; }
    [StringLength(20)] public string? FitchRating { get; set; }
    [Required, Range(1, 255)] public byte OrderNumber { get; set; }
}