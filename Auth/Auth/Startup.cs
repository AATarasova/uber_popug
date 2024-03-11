using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Auth.DAL;
using Auth.DAL.Context;
using Auth.Domain;
using Auth.Domain.Users.Management;
using EventsManager.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using SchemaRegistry;

namespace Auth;

public class Startup
{
    public void ConfigureServices(IServiceCollection serviceCollection, IConfiguration builderConfiguration)
    {
        serviceCollection.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy=JsonNamingPolicy.CamelCase;
        });
        
        serviceCollection.AddEndpointsApiExplorer();
        var assembly = typeof(Startup).GetTypeInfo().Assembly;
        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        serviceCollection.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName?.Replace('+','.'));
        });
    
        serviceCollection.AddDbContext<AuthDbContext>();
        serviceCollection.AddSchemaRegistry();
        serviceCollection.RegisterAuthDAL();
        serviceCollection.RegisterAuthDomain();
        serviceCollection.RegisterEventsDomain(builderConfiguration);

        serviceCollection.AddScoped<EventsFactory>();
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