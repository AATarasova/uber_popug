using Auth.Domain.Roles;
using Auth.Domain.Users;
using Auth.Domain.Users.Management;
using EventManager.Domain.Producer;
using JetBrains.Annotations;
using MediatR;

namespace Auth.Users;

public static class CreateUserData
{
    public record Command(Args Args) : IRequest;
    
    [PublicAPI]
    public class Args
    {
        public string Password { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string FirstName { get; init; } = null!;
        public string MiddleName { get; init; } = null!;
        public Role Role { get; init; }
    }
    
    public class Handler(IUserManager userManager) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var template = new CreateUserDto
            {
                LastName = request.Args.LastName,
                FirstName = request.Args.FirstName,
                MiddleName = request.Args.MiddleName,
                Email = request.Args.Email,
                Password = request.Args.Password,
                Role = request.Args.Role
            };
            await userManager.Create(template);
        }
    }
}