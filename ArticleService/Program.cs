using ArticleService;
using ArticleService.Database;
using ArticleService.Services;
using Microsoft.AspNetCore.Builder;

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

builder.Services.AddDbContext<ArticleDbContext>();
builder.Services.AddSingleton<Coordinator>();
builder.Services.AddScoped<IArticleRepository, ArticleDatabase>();
builder.Services.AddScoped<IArticleService, ArticleService.Services.ArticleService>();

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