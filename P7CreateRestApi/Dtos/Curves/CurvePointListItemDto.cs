namespace P7CreateRestApi.Dtos.Curves;

public class CurvePointListItemDto
{
    public int Id { get; set; }
    public byte CurveId { get; set; }
    public double CurvePointValue { get; set; }
}