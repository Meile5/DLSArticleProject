using ArticleService;
using ArticleService.AppOptionsPattern;
using ArticleService.BackgroundServices;
using ArticleService.Database;
using ArticleService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddMessageClient(
    appOptions.ConnectionStrings["RabbitMqConstring"]
);
builder.Services.AddHostedService<EasyNetQSubscriberService>();

builder.Services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("DevelopmentCorsPolicy");

await app.RunAsync();