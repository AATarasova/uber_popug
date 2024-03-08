using Auth.Domain.Users.Management;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Domain;

public static class Registrar
{
    public static void RegisterAuthDomain(this IServiceCollection services)
    {
        services.AddScoped<IUserManager, UserManager>();
    }
}