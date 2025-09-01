using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Dtos.Trades;

public class CreateTradeRequestDto
{
    [Required, StringLength(50)] public string Account { get; set; } = default!;
    [StringLength(30)] public string? AccountType { get; set; } = default!;
    [Range(0, double.MaxValue)] public double? BuyQuantity { get; set; }
    [Range(0, double.MaxValue)] public double? SellQuantity { get; set; }
    [Range(0, double.MaxValue)] public double? BuyPrice { get; set; }
    [Range(0, double.MaxValue)] public double? SellPrice { get; set; }
    public DateTime? TradeDate { get; set; }
    [StringLength(60)] public string? TradeSecurity { get; set; }
    [StringLength(30)] public string? TradeStatus { get; set; }
    [StringLength(60)] public string? Trader { get; set; }
    [StringLength(60)] public string? Benchmark { get; set; }
    [StringLength(30)] public string? Book { get; set; }
    [StringLength(60)] public string? CreationName { get; set; }
    public DateTime? CreationDate { get; set; }
    [StringLength(60)] public string? RevisionName { get; set; }
    public DateTime? RevisionDate { get; set; }
    [StringLength(60)] public string? DealName { get; set; }
    [StringLength(60)] public string? DealType { get; set; }
    [StringLength(60)] public string? SourceListId { get; set; }
    [StringLength(10)] public string? Side { get; set; }
}