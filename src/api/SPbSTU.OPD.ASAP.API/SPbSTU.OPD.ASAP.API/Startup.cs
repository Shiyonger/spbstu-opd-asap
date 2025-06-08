using FluentValidation;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Options;
using SPbSTU.OPD.ASAP.API.Application.Extensions;
using SPbSTU.OPD.ASAP.API.Domain.Contracts.Grpc;
using SPbSTU.OPD.ASAP.API.Extensions;
using SPbSTU.OPD.ASAP.API.Infrastructure;
using SPbSTU.OPD.ASAP.API.Infrastructure.Extensions;
using SPbSTU.OPD.ASAP.API.Infrastructure.Grpc;
using SPbSTU.OPD.ASAP.API.Infrastucture.Settings;

namespace SPbSTU.OPD.ASAP.API;

public sealed class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddLogging()
            .AddCors()
            .AddGrpc();

        services
            .AddGrpcClient<UsersService.UsersServiceClient>(o => { o.Address = new Uri(configuration["GrpcUri"]!); })
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true);
        services.AddGrpcClient<CoursesService.CoursesServiceClient>(o =>
            {
                o.Address = new Uri(configuration["GrpcUri"]!);
            })
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true);
        services.AddGrpcClient<AssignmentsService.AssignmentsServiceClient>(o =>
            {
                o.Address = new Uri(configuration["GrpcUri"]!);
            })
            .EnableCallContextPropagation(o => o.SuppressContextNotFoundErrors = true);

        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        services.AddApiAuthentication(
            configuration.GetSection(nameof(JwtOptions))
                .Get<JwtOptions>()!);

        services
            .AddInfrastructure()
            .AddApplicationsServices();

        services.AddControllers();

        services.AddValidatorsFromAssemblyContaining<Program>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseCors(x =>
            x.WithOrigins(configuration["FrontendUri"]!).AllowAnyHeader().AllowAnyMethod().AllowCredentials());

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCookiePolicy(new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
            HttpOnly = HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.Always
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}