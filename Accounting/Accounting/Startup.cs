using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Accounting.AccessRights;
using Accounting.Consumer;
using Accounting.Consumer.EmployeeCreated;
using Accounting.Consumer.EmployeeRoleChanged;
using Accounting.Consumer.TaskAssigned;
using Accounting.Consumer.TaskCreated;
using Accounting.DAL;
using Accounting.DAL.Context;
using Accounting.Domain;
using Accounting.WorkingDay;
using EventsManager.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using SchemaRegistry;

namespace Accounting;

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
        
        serviceCollection.AddDbContext<AccountingDbContext>();
        serviceCollection.RegisterDAL();
        serviceCollection.RegisterDomain();
        serviceCollection.RegisterEventsDomain(configuration);
        serviceCollection.AddSchemaRegistry();

        serviceCollection.AddSingleton<EmployeeRoleChangedHandler>()
            .AddSingleton<EmployeeCreatedHandler>()
            .AddSingleton<TaskStatusChangedHandler>()
            .AddSingleton<TaskCreatedHandler>()
            .AddTransient<AccessRightsManager>()
            .AddScoped<EventsFactory>()
            .AddScoped<ProducedEventsFactory>();
        
        serviceCollection.AddHostedService<EventsConsumer>();
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