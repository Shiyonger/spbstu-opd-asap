using SPbSTU.OPD.ASAP.API;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
    .Build();

host.Run();