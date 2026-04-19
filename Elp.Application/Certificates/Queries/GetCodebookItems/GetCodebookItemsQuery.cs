using MediatR;

namespace Elp.Application.Certificates.Queries.GetCodebookItems;

public record GetCodebookItemsQuery(string CodebookCode, string LanguageCode) : IRequest<List<CodebookItemDto>?>;