using System.ComponentModel.DataAnnotations;

namespace ProfanityService.AppOptionsPattern;

public sealed class AppOptions
{
    [Required] public string DbConnectionString { get; set; } = string.Empty!;

}