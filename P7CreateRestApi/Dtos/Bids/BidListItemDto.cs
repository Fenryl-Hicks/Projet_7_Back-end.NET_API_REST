namespace P7CreateRestApi.Dtos.Bids;

public class BidListItemDto
{
    public int Id { get; set; }
    public string Account { get; set; } = default!;
    public double BidQuantity { get; set; }
}