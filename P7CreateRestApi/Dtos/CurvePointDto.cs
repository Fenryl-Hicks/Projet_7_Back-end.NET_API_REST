using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.ClassDto
{
    public class CurvePointDto
    {
        [Required(ErrorMessage = "CurveId est requis.")]
        [Range(1, byte.MaxValue, ErrorMessage = "CurveId doit être compris entre 1 et 300.")]
        public byte? CurveId { get; set; }

        
        [DataType(DataType.Date, ErrorMessage = "AsOfDate invalide.")]
        public DateTime? AsOfDate { get; set; }

        [Required(ErrorMessage = "CurvePointValue est requis.")]
        [Range(double.MinValue, double.MaxValue, ErrorMessage = "Valeur invalide pour CurvePointValue.")]
        public double? CurvePointValue { get; set; }
    }
}
