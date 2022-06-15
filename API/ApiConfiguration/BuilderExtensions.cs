using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace API.ApiConfiguration
{
  public static class BuilderExtensions
  {
    public static void AddSerilog(this ConfigureHostBuilder host)
    {
      host.UseSerilog();

      Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
          .Enrich.FromLogContext()
          .WriteTo.Console()
          .WriteTo.File(new CompactJsonFormatter(), "logs.log", rollingInterval: RollingInterval.Day, flushToDiskInterval: new TimeSpan(0,0,10))
          .CreateLogger();
    }
  }
}
