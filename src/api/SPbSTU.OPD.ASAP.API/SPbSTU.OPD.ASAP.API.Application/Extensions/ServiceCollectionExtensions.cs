using Microsoft.Extensions.DependencyInjection;
using SPbSTU.OPD.ASAP.API.Application.Services;
using SPbSTU.OPD.ASAP.API.Domain.Contracts;
using SPbSTU.OPD.ASAP.API.Domain.Contracts.Services;

namespace SPbSTU.OPD.ASAP.API.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationsServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}