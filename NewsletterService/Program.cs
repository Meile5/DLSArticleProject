using ArticleQueue.Extensions;
using ArticleQueue.Models.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var options = builder.Services.MessageClientOptions(builder.Configuration);
builder.Services.AddRabbitMqMessageClient(options);
builder.Services.AddMessagingHandlers(typeof(Program).Assembly);
builder.Services.AddSubscription<ArticlePublishedEvent>("article-newsletter");
builder.Services.AddScoped<NewsletterService.Services.NewsletterService>(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();
