using ArticleQueue.Extensions;
using ArticleService;
using ArticleService.AppOptionsPattern;
using ArticleService.BackgroundServices;
using ArticleService.Database;
using ArticleService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCorsPolicy", corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var appOptions = builder.Services.AddAppOptions(builder.Configuration);

builder.Services.AddSingleton<Coordinator>();

builder.Services.AddDbContext<ArticleDbContext>(options =>
{
    options.UseSqlServer(appOptions.Shards["Global"]);
});

builder.Services.AddScoped<IArticleRepository, ArticleDatabase>();
builder.Services.AddScoped<IArticleService, ArticleService.Services.ArticleService>();

var options = builder.Services.MessageClientOptions(builder.Configuration);
 
builder.Services.AddRabbitMqMessageClient(options);

builder.Services.AddHostedService<ArticleSubscriber>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseCors("DevelopmentCorsPolicy");

app.MapControllers();

await app.RunAsync();