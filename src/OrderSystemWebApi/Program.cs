using OrderSystemWebApi;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try 
{
    await Startup.InitializeApplication(args);
}
catch (Exception e)
{
    Log.Fatal($"Fatal error {e}");
}
finally
{
    Log.CloseAndFlush();
}