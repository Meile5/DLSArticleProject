using MonitorService;
using NewsletterService.AppOptionsPattern;
using NewsletterService.Clients;
using OpenTelemetry.Trace;
using Shared.Events;
using SubscriberQueue.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var options = builder.Services.MessageClientOptions(builder.Configuration);

builder.Services.AddOpenTelemetry().Setup();
builder.Services.AddSingleton(TracerProvider.Default.GetTracer(Monitoring.ActivitySource.Name));

builder.Services.AddRabbitMqMessageClient(options);
builder.Services.AddMessagingHandlers(typeof(Program).Assembly);
builder.Services.AddSubscription<ArticlePublishedEvent>("article-newsletter");
builder.Services.AddSubscription<NewSubscriberSuccessEvent>("new-subscriber");
builder.Services.AddScoped<NewsletterService.Services.NewsletterService>();

var appOptions = builder.Services.AddAppOptions(builder.Configuration);

// Subscriber HTTP client wrapped with Polly resilience pipeline
// (retry + circuit breaker configured inside SubscriberClient)
builder.Services.AddHttpClient<ISubscriberClient, SubscriberClient>(client =>
{
    client.BaseAddress = new Uri(appOptions.SubscriberService.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(appOptions.SubscriberService.TimeoutSeconds);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();
