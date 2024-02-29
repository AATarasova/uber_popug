using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Domain.Credentials;
using Auth.Domain.Users;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Authentication;

public static class Authenticate
{
    public record Command(Args Args) : IRequest<IActionResult>;
    
    [PublicAPI]
    public class Args
    {
        public string Password { get; init; } = null!;
        public string Email { get; init; } = null!;
    }
    
    
    public class Handler: IRequestHandler<Command, IActionResult>
    {
        private readonly ICredentialsService _credentialsService;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public Handler(ICredentialsService credentialsService, 
            IConfiguration configuration,
            IUserRepository userRepository)
        {
            _credentialsService = credentialsService;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<IActionResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = await _credentialsService.FindUser(request.Args.Email, request.Args.Password);
            if (!userId.HasValue)
            {
                return new BadRequestObjectResult("Invalid credentials");
            }
            
            var user = await _userRepository.GetById(userId.Value);
            
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim("UserId", user.PublicId.ToString()),
                new Claim("Role", user.Role.ToString()),
            };

            var securityKey  = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials  = new SigningCredentials(securityKey , SecurityAlgorithms.HmacSha512Signature);
            
            var tokenHandler = new JwtSecurityTokenHandler();             
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = credentials
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new OkObjectResult(tokenHandler.WriteToken(token));
        }
    }
}