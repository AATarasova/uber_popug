using Auth.Domain.Roles;
using Auth.Domain.Users;
using JetBrains.Annotations;
using MediatR;

namespace Auth.Users;

public static class UpdateRole
{
    public record Command(int UserId, Args Args) : IRequest;

    [PublicAPI]
    public record Args(Role Role);
    
    public class Handler: IRequestHandler<Command>
    {
        private readonly IRolesManager _rolesManager;

        public Handler(IRolesManager rolesManager)
        {
            _rolesManager = rolesManager;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await _rolesManager.ChangeRole(new UserId(request.UserId), request.Args.Role);
        }
    }
}