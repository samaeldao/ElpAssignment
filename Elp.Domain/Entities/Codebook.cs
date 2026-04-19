using System.ComponentModel.DataAnnotations;

namespace Elp.Domain.Entities;

public class Codebook
{
    [Key]
    public string Code { get; set; } = string.Empty;

    public string NameCz { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;

    public ICollection<CodebookItem> Items { get; set; } = new List<CodebookItem>();
}