using Auth.Domain.Roles;
using Auth.Domain.Users;
using Auth.Domain.Users.Management;
using JetBrains.Annotations;
using MediatR;

namespace Auth.Users;

public static class UpdateRole
{
    public record Command(int UserId, Args Args) : IRequest;

    [PublicAPI]
    public record Args(Role Role);
    
    public class Handler(IUserManager manager) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await manager.UpdateRole(new UserId(request.UserId), request.Args.Role);
        }
    }
}