using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EventsManager.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using TaskTracker.Consumer.Users;
using TaskTracker.DAL;
using TaskTracker.DAL.Context;
using TaskTracker.Domain;

namespace TaskTracker;

public class Startup
{
    public void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
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
        
        serviceCollection.AddDbContext<TaskTrackerDbContext>();
        serviceCollection.RegisterDomain();
        serviceCollection.RegisterDAL();
        serviceCollection.RegisterEventsDomain(configuration);
        
        serviceCollection.AddHostedService<UserEventsConsumer>();
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
                    ValidateIssuer = false,
                    ValidateAudience = false,
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