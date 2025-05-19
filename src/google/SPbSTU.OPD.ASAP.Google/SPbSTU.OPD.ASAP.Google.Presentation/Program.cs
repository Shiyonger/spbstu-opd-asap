namespace SPbSTU.OPD.ASAP.Google;

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
            .Build();

        host.Run();
    }
}