using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Dtos.RuleNames;

public class CreateRuleNameRequestDto
{
    [Required, StringLength(100)] public string Name { get; set; } = default!;
    [StringLength(250)] public string? Description { get; set; }
    public string? Json { get; set; }
    public string? Template { get; set; }
    public string? SqlStr { get; set; }
    public string? SqlPart { get; set; }
}