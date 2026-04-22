using RabbitMQ.Client;
using SubscriberService.AppOptionsPattern;
using SubscriberService.Database;
using SubscriberService.Services;
using ArticleQueue.Extensions;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appOptions = builder.Services.AddAppOptions(builder.Configuration);

// Azure App Configuration
var azureAppConfigConnection = 
    builder.Configuration.GetConnectionString("AzureAppConfig") 
    ?? Environment.GetEnvironmentVariable("AZURE_APP_CONFIG");

if (!string.IsNullOrWhiteSpace(azureAppConfigConnection))
{
    builder.Configuration.AddAzureAppConfiguration(azureOptions =>
    {
        azureOptions.Connect(azureAppConfigConnection)
            .UseFeatureFlags(flagOptions =>
            {
                flagOptions.SetRefreshInterval(TimeSpan.FromSeconds(30));
            });
    });
    builder.Services.AddAzureAppConfiguration(); 
}



builder.Services.AddFeatureManagement();       

builder.Services.AddScoped<ISubscriberRepository, SubscriberDatabase>();
builder.Services.AddScoped<ISubscriberService, SubscriberService.Services.SubscriberService>();

builder.Services.AddSingleton<IConnection>(_ =>
{
    var factory = new ConnectionFactory
    {
        HostName = appOptions.RabbitMqHost
    };
    return factory.CreateConnection();
});

var options = builder.Services.MessageClientOptions(builder.Configuration);
builder.Services.AddRabbitMqMessageClient(options);

var app = builder.Build();
app.UseAzureAppConfiguration(); // add before UseHttpsRedirection

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();