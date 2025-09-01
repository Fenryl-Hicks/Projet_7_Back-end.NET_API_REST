using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.ClassDto
{
    public class TradeDto : IValidatableObject
    {
        [Required(ErrorMessage = "Account est requis.")]
        [StringLength(50, ErrorMessage = "Account ne doit pas dépasser 50 caractères.")]
        public string Account { get; set; }

        [Required(ErrorMessage = "AccountType est requis.")]
        [StringLength(30, ErrorMessage = "AccountType ne doit pas dépasser 30 caractères.")]
        public string AccountType { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "BuyQuantity ne peut pas être négatif.")]
        public double? BuyQuantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "SellQuantity ne peut pas être négatif.")]
        public double? SellQuantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "BuyPrice ne peut pas être négatif.")]
        public double? BuyPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "SellPrice ne peut pas être négatif.")]
        public double? SellPrice { get; set; }

        [DataType(DataType.Date, ErrorMessage = "TradeDate invalide.")]
        public DateTime? TradeDate { get; set; }

        [StringLength(60)] public string TradeSecurity { get; set; }
        [StringLength(30)] public string TradeStatus { get; set; }
        [StringLength(60)] public string Trader { get; set; }
        [StringLength(60)] public string Benchmark { get; set; }
        [StringLength(30)] public string Book { get; set; }
        [StringLength(60)] public string CreationName { get; set; }
        public DateTime? CreationDate { get; set; }
        [StringLength(60)] public string RevisionName { get; set; }
        public DateTime? RevisionDate { get; set; }
        [StringLength(60)] public string DealName { get; set; }
        [StringLength(60)] public string DealType { get; set; }
        [StringLength(60)] public string SourceListId { get; set; }
        [StringLength(10)] public string Side { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            
            if (SellQuantity.HasValue && SellQuantity.Value > 0 && (!SellPrice.HasValue || SellPrice.Value <= 0))
            {
                yield return new ValidationResult(
                    "SellPrice doit être > 0 quand SellQuantity est renseigné.",
                    new[] { nameof(SellPrice), nameof(SellQuantity) });
            }

            
            if (BuyQuantity.HasValue && BuyQuantity.Value > 0 && (!BuyPrice.HasValue || BuyPrice.Value <= 0))
            {
                yield return new ValidationResult(
                    "BuyPrice doit être > 0 quand BuyQuantity est renseigné.",
                    new[] { nameof(BuyPrice), nameof(BuyQuantity) });
            }
        }
    }
}
