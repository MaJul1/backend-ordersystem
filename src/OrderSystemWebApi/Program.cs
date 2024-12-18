using OrderSystemWebApi;
using Serilog;
using Serilog.Core;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try 
{
    await Startup.InitializeApplication(args);
}
catch (Exception e)
{
    Log.Fatal($"Fatal error{e.Message}");
}
finally
{
    Log.CloseAndFlush();
}