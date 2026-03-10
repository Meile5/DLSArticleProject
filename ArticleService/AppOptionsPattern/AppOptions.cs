using System.ComponentModel.DataAnnotations;

namespace ArticleService.AppOptionsPattern;

public sealed class AppOptions
{
    [Required]
    public Dictionary<string, string> ConnectionStrings { get; set; } = new();

    [Required]
    public Dictionary<string, string> Shards { get; set; } = new();
}