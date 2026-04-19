using Elp.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elp.Application.Certificates.Queries.GetCodebooks;

public class GetCodebooksQueryHandler : IRequestHandler<GetCodebooksQuery, List<CodebookDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCodebooksQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<CodebookDto>> Handle(GetCodebooksQuery request, CancellationToken cancellationToken)
    {
        // Check if the requested language is English
        bool isEnglish = request.LanguageCode.StartsWith("en", StringComparison.OrdinalIgnoreCase);

        // Query the database and select the correct translation
        return await _context.Codebooks
            .AsNoTracking()
            .Select(c => new CodebookDto
            {
                Code = c.Code,
                Name = isEnglish ? c.NameEn : c.NameCz
            })
            .ToListAsync(cancellationToken);
    }
}