using Accounting.Domain.Payment;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Domain;

public static class Registrar
{
    public static void RegisterDomain(this IServiceCollection services)
    {
        services.AddScoped<IPaymentService, PaymentService>();
    }
}