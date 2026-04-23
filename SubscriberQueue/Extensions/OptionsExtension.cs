using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SubscriberQueue.Configuration;

namespace SubscriberQueue.Extensions;

public static class OptionsExtension
{
    public static MessageClientOptions MessageClientOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var appOptions = new MessageClientOptions();
        configuration.GetSection("MessageClientOptions").Bind(appOptions);
        services.Configure<MessageClientOptions>(configuration.GetSection("MessageClientOptions"));

        ICollection<ValidationResult> results = new List<ValidationResult>();
        var validated = Validator.TryValidateObject(appOptions, new ValidationContext(appOptions), results, true);
        if (!validated)
            throw new Exception(
                "You're probably missing an environment variable / appsettings.json / repo secret on github. " +
                $"Here's the technical error: {string.Join(", ", results.Select(r => r.ErrorMessage))}");

        return appOptions;
    }
}
