namespace P7CreateRestApi.Dtos.Bids;

public class BidResponseDto
{
    public int Id { get; set; }
    public string Account { get; set; } = default!;
    public string BidType { get; set; } = default!;
    public double BidQuantity { get; set; }
}