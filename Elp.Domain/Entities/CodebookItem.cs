using System.ComponentModel.DataAnnotations;

namespace Elp.Domain.Entities;

public class CodebookItem
{
    [Key]
    public string Code { get; set; } = string.Empty;

    public string DescriptionCz { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public string CodebookCode { get; set; } = string.Empty;
    public Codebook Codebook { get; set; } = null!;
}