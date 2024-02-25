using Auth.Domain.Users;
using JetBrains.Annotations;
using MediatR;

namespace Auth.Users;

public static class ListUserData
{
    public record Query: IRequest<Response>;
    [PublicAPI]
    public record Response(IEnumerable<User> Users);
    [PublicAPI]
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    
    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IUserRepository _userRepository;

        public Handler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
        {
            var templates = await _userRepository.ListAll();

            var dtos = templates.Select(t => new User
            {
                Id = t.PublicId,
                UserName = t.UserName.FullName,
                Email = t.Email,
                Password = t.Password
            });
            return new Response(dtos);
        }
    }
}