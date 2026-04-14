using ArticleQueue.Extensions;
using ArticleQueue.Models.Events;
using SubscriberQueue;
using SubscriberQueue.Events;
using SubscriberQueue.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
    
var options = builder.Services.MessageClientOptions(builder.Configuration);

builder.Services.AddRabbitMqMessageClient(options);
builder.Services.AddMessagingHandlers(typeof(Program).Assembly);
builder.Services.AddSubscription<NewSubscriberEvent>("new-subscriber");
builder.Services.AddScoped<SubscriberList>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
