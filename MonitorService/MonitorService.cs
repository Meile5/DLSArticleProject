using System.Diagnostics;
using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;
using ILogger = Serilog.ILogger;


namespace MonitorService;

public static class Monitoring
{
    public static TracerProvider TracerProvider;
    public static readonly ActivitySource ActivitySource = new("MonitorService", "1.0.0"); //used everytime a new method is called
    
    //public static ILogger Log => Serilog.Log.Logger;
    
    //static constructor
    //runs before the first instance is created or any static methods are called
    static Monitoring()
    {
        var serviceName = Assembly.GetExecutingAssembly().GetName().Name;
        var version = "1.0.0";
        
        //OpenTelemetry Setup (Tracing)
        TracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddZipkinExporter()
            .AddConsoleExporter()
            .AddSource(ActivitySource.Name)
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName: serviceName, serviceVersion: version))
            .Build();
        
        //Serilog Setup (Debug, Logs, also can get trace Ids )
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.WithSpan()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341") //if you change the PORT make sure to change it here too
            .CreateLogger();
        
        //if you want to see the logs in the seq website (for debugging)
        //run the docker container for this service and go to localhost:5342
        //or whatever port you assigned to port 80 in the docker-compose
        
    }
}