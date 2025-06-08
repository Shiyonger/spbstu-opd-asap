using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace SPbSTU.OPD.ASAP.Google;

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.ConfigureKestrel(options =>
                    options.ListenAnyIP(5243, listenOptions => listenOptions.Protocols = HttpProtocols.Http2));
                builder.UseStartup<Startup>();
            })
            .Build();

        host.Run();
    }
}