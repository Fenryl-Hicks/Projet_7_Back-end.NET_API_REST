
namespace P7CreateRestApi.Dtos.RuleNames;
public class RuleNameResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Json { get; set; }
    public string? Template { get; set; }
    public string? SqlStr { get; set; }
    public string? SqlPart { get; set; }
}