namespace P7CreateRestApi.Dtos.Trades;

public class TradeListItemDto
{
    public int Id { get; set; }
    public string Account { get; set; } = default!;
    public double? BuyQuantity { get; set; }
    public double? SellQuantity { get; set; }
}