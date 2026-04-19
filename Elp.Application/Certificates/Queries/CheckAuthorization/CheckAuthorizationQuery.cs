using MediatR;

namespace Elp.Application.Certificates.Queries.CheckAuthorization
{
    public record CheckAuthorizationQuery(string KrzpId, string Rid) : IRequest<bool>;
}
