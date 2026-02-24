using System.ComponentModel.DataAnnotations;

namespace ArticleService.AppOptionsPattern;

public sealed class AppOptions
{
    public Dictionary<string, string> ConnectionStrings { get; set; } = new();

}