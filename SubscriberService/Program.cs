using RabbitMQ.Client;
using SubscriberService.AppOptionsPattern;
using SubscriberService.Database;
using SubscriberService.Services;
using ArticleQueue.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appOptions = builder.Services.AddAppOptions(builder.Configuration);

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

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();