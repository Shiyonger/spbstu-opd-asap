using FluentMigrator.Runner;
using SPbSTU.OPD.ASAP.Core;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
    .Build();

if (args.Length > 0 && args[0].Equals("migrate", StringComparison.InvariantCultureIgnoreCase))
{
    using var scope = host.Services.CreateScope();
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

    runner.MigrateUp();
}
else
{
    host.Run();
}
host.Run();