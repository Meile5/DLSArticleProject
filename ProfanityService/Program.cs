using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProfanityService;

var builder = Host.CreateApplicationBuilder(args);

var services = builder.Services;

services.AddHostedService<Worker>();

/*
services.AddDbContext<AppDbContext>((service, options) =>
{
    var provider = services.BuildServiceProvider();
    options.UseNpgsql(
        provider.GetRequiredService<IOptionsMonitor<AppOptions>>().CurrentValue.DbConnectionString);
    options.EnableSensitiveDataLogging();
});
*/

var host = builder.Build();
host.Run();