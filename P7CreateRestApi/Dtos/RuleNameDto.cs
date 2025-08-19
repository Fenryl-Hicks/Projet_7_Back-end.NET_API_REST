using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.ClassDto
{
    public class RuleNameDto
    {
        [Required(ErrorMessage = "Name est requis.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name doit contenir entre 3 et 50 caractères.")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Description ne doit pas dépasser 200 caractères.")]
        public string Description { get; set; }

        
        [Required(ErrorMessage = "Json est requis.")]
        [StringLength(2000, ErrorMessage = "Json ne doit pas dépasser 2000 caractères.")]
        public string Json { get; set; }

        [Required(ErrorMessage = "Template est requis.")]
        [StringLength(500, ErrorMessage = "Template ne doit pas dépasser 500 caractères.")]
        public string Template { get; set; }

        [Required(ErrorMessage = "SqlStr est requis.")]
        [StringLength(2000, ErrorMessage = "SqlStr ne doit pas dépasser 2000 caractères.")]
        public string SqlStr { get; set; }

        [StringLength(1000, ErrorMessage = "SqlPart ne doit pas dépasser 1000 caractères.")]
        public string SqlPart { get; set; }
    }
}
