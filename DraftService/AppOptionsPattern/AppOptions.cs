using System.ComponentModel.DataAnnotations;

namespace DraftService.AppOptionsPattern;

public sealed class AppOptions
{
    [Required] public string DbConnectionString { get; set; } = string.Empty!;

}