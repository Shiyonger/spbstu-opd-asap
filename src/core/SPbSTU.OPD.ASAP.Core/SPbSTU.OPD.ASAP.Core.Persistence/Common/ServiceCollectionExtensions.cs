using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Npgsql.NameTranslation;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;
using SPbSTU.OPD.ASAP.Core.Persistence.Entities;
using SPbSTU.OPD.ASAP.Core.Persistence.Repositories;

namespace SPbSTU.OPD.ASAP.Core.Persistence.Common;

public static class ServiceCollectionExtensions
{
    private static readonly INpgsqlNameTranslator Translator = new NpgsqlSnakeCaseNameTranslator();

    public static void MapCompositeTypes(this IServiceCollection services)
    {
        var mapper = NpgsqlConnection.GlobalTypeMapper;
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        mapper.MapComposite<OutboxPointsEntityV1>("outbox_points_v1", Translator);
        mapper.MapComposite<OutboxQueueEntityV1>("outbox_queue_v1", Translator);
    }

    public static IServiceCollection AddFluentMigrator(
        this IServiceCollection services,
        string connectionString,
        Assembly assembly)
    {
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(
                builder => builder
                    .AddPostgres()
                    .ScanIn(assembly).For.Migrations())
            .AddOptions<ProcessorOptions>()
            .Configure(
                options =>
                {
                    options.ProviderSwitches = "Force Quote=false";
                    options.Timeout = TimeSpan.FromMinutes(10);
                    options.ConnectionString = connectionString;
                });

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IOutboxPointsRepository, OutboxPointsRepository>(_ =>
            new OutboxPointsRepository(connectionString));
        services.AddScoped<IOutboxQueueRepository, OutboxQueueRepository>(_ =>
            new OutboxQueueRepository(connectionString));
        services.AddScoped<IUsersRepository, UsersRepository>(_ => 
            new UsersRepository(connectionString));
        services.AddScoped<IAssignmentsRepository, AssignmentsRepository>(_ => 
            new AssignmentsRepository(connectionString));
        services.AddScoped<ICoursesRepository, CoursesRepository>(_ => 
            new CoursesRepository(connectionString));
        services.AddScoped<IGoogleRepository, GoogleRepository>(_ => 
            new GoogleRepository(connectionString));
        services.AddScoped<ISubmissionsRepository, SubmissionsRepository>(_ => 
            new SubmissionsRepository(connectionString));
        services.AddScoped<IGithubRepository, GithubRepository>(_ => 
            new GithubRepository(connectionString));

        return services;
    }
}