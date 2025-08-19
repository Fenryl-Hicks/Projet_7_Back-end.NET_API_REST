using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.ClassDto
{
    public class BidDto
    {
        [Required(ErrorMessage = "Account est requis.")]
        [StringLength(50, ErrorMessage = "Account ne doit pas dépasser 50 caractères.")]
        public string Account { get; set; }

        [Required(ErrorMessage = "BidType est requis.")]
        [StringLength(30, ErrorMessage = "BidType ne doit pas dépasser 30 caractères.")]
        public string BidType { get; set; }

        [Required(ErrorMessage = "BidQuantity est requise.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "BidQuantity doit être > 0.")]
        public double? BidQuantity { get; set; }
    }
}
