using MediatR;

namespace Elp.Application.Certificates.Queries.GetCodebooks;

public record GetCodebooksQuery(string LanguageCode) : IRequest<List<CodebookDto>>;