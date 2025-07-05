using Microsoft.Extensions.DependencyInjection;
using SPbSTU.OPD.ASAP.API.Domain.Contracts;
using SPbSTU.OPD.ASAP.API.Domain.Contracts.Auth;
using SPbSTU.OPD.ASAP.API.Domain.Contracts.Grpc;
using SPbSTU.OPD.ASAP.API.Infrastructure.Authorization;
using SPbSTU.OPD.ASAP.API.Infrastructure.Grpc;

namespace SPbSTU.OPD.ASAP.API.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUsersGrpcService, UsersGrpcService>();
        services.AddScoped<IAssignmentsGrpcService, AssignmentsGrpcService>();
        services.AddScoped<ICoursesGrpcService, CoursesGrpcService>();
        
        return services;
    }
}