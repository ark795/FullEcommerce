using Microsoft.AspNetCore.Builder;
using Serilog;

namespace BuildingBlocks.Infrastructure.Logging;

public static class SerilogConfiguration
{
    public static void AddSerilogLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Host.UseSerilog();
    }
}
