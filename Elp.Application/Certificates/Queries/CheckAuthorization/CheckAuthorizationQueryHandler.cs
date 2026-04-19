using MediatR;

namespace Elp.Application.Certificates.Queries.CheckAuthorization
{
    public class CheckAuthorizationQueryHandler : IRequestHandler<CheckAuthorizationQuery, bool>
    {
        public Task<bool> Handle(CheckAuthorizationQuery request, CancellationToken cancellationToken)
        {
            // TODO : In reality, check the doctor's license status against a government database.
            return Task.FromResult(true);
        }
    }
}
