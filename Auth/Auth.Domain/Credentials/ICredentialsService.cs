using Auth.Domain.Users;

namespace Auth.Domain.Credentials;

public interface ICredentialsService
{
    Task<UserId?>  FindUser(string email, string password);
}