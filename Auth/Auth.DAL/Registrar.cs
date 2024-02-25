using Auth.DAL.Repository;
using Auth.Domain.Credentials;
using Auth.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.DAL;

public static class Registrar
{
    public static void RegisterAuthDAL(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICredentialsService, UserRepository>();
    }
}