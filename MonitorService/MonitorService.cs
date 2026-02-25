using System.Diagnostics;
using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;
using ILogger = Serilog.ILogger;


namespace MonitorService;

public class MonitorService
{
    public static readonly string ServiceName = Assembly.GetCallingAssembly().GetName().Name ?? "unknown";
    public static TracerProvider TracerProvider;
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName); //used everytime a new method is called
    
    public static ILogger Log => Serilog.Log.Logger;
    
    //static constructor
    //runs before the first instance is created or any static methods are called
    static MonitorService()
    {
        //OpenTelemetry Setup (Tracing)
        //can't seem to get this to work, probably because zipkin library is depreciated
        TracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource(ActivitySource.Name)
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ServiceName))
            .AddConsoleExporter()
            .AddOtlpExporter(config =>
            {
                config.Endpoint = new Uri("http://localhost:4317");
            })
            .AddZipkinExporter()
            .Build();
        
        //Serilog Setup (Debug, Logs, also tracing)
        Serilog.Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341") //if you change the PORT make sure to change it here too
            .Enrich.WithSpan()
            .CreateLogger();
        
        //if you want to see the logs in the seq website (for debugging)
        //run the docker container for this service and go to localhost:5342
        //or whatever port you assigned to port 80 in the docker-compose
        
    }
}