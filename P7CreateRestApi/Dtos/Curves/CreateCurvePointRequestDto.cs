using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Dtos.Curves;

public class CreateCurvePointRequestDto
{
    [Required, Range(1, byte.MaxValue)] public byte CurveId { get; set; }
    public DateTime? AsOfDate { get; set; }
    [Required] public double CurvePointValue { get; set; }
}