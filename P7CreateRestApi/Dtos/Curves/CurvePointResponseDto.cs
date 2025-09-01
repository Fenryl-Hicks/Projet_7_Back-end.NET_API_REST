namespace P7CreateRestApi.Dtos.Curves;

public class CurvePointResponseDto
{
    public int Id { get; set; }
    public byte CurveId { get; set; }
    public DateTime? AsOfDate { get; set; }
    public double CurvePointValue { get; set; }
}