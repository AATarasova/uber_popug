using System.Reflection;
using System.Text;
using Auth.DAL;
using Auth.DAL.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Auth;

public class Startup
{
    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddControllers();
        
        serviceCollection.AddEndpointsApiExplorer();
        var assembly = typeof(Startup).GetTypeInfo().Assembly;
        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        serviceCollection.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName?.Replace('+','.'));
        });
        
        serviceCollection.AddDbContext<AuthDbContext>();
        serviceCollection.RegisterAuthDAL();
        
        
    }

    public void ConfigureAuth(IServiceCollection serviceCollection, ConfigurationManager configuration)
    {
        serviceCollection
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });
        var multiSchemePolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)  
            .RequireAuthenticatedUser()  
            .Build();  
        serviceCollection.AddAuthorization(o => o.DefaultPolicy = multiSchemePolicy); 
    }
    
}