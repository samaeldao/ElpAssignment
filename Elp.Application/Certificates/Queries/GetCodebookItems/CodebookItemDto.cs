namespace Elp.Application.Certificates.Queries.GetCodebookItems;

public class CodebookItemDto
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}