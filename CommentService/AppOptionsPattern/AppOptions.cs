using System.ComponentModel.DataAnnotations;

namespace CommentService.AppOptionsPattern;

public sealed class AppOptions
{
    [Required] public string DbConnectionString { get; set; } = string.Empty!;

}