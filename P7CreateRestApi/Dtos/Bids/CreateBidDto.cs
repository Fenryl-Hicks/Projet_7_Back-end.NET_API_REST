using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Dtos.Bids;

public class CreateBidRequestDto
{
    [Required, StringLength(50)] public string Account { get; set; } = default!;
    [Required, StringLength(30)] public string BidType { get; set; } = default!;
    [Required, Range(0.0001, double.MaxValue)] public double BidQuantity { get; set; }
}