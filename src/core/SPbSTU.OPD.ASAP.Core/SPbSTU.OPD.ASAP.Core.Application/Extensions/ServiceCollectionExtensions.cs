using Microsoft.Extensions.DependencyInjection;
using SPbSTU.OPD.ASAP.Core.Application.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;

namespace SPbSTU.OPD.ASAP.Core.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IOutboxService, OutboxService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IAssignmentsService, AssignmentsService>();
        services.AddScoped<ICoursesService, CoursesService>();

        return services;
    }
}