using MediatR;
using Elp.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Elp.Application.Certificates.Queries.GetCodebookItems;

public class GetCodebookItemsQueryHandler : IRequestHandler<GetCodebookItemsQuery, List<CodebookItemDto>?>
{
    private readonly IApplicationDbContext _context;

    public GetCodebookItemsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<CodebookItemDto>?> Handle(GetCodebookItemsQuery request, CancellationToken cancellationToken)
    {
        var codebookExists = await _context.Codebooks.AnyAsync(c => c.Code == request.CodebookCode, cancellationToken);
        if (!codebookExists) return null;

        // Determine if English is requested (e.g., "en-US", "en")
        bool isEnglish = request.LanguageCode.StartsWith("en", StringComparison.OrdinalIgnoreCase);

        return await _context.CodebookItems
            .AsNoTracking()
            .Where(ci => ci.CodebookCode == request.CodebookCode)
            .Select(ci => new CodebookItemDto
            {
                Code = ci.Code,
                // Dynamically select the correct column based on the language
                Description = isEnglish ? ci.DescriptionEn : ci.DescriptionCz,
                IsActive = ci.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}