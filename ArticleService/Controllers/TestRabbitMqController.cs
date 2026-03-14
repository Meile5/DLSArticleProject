using ArticleQueue.Interfaces;
using ArticleQueue.Models.Events;
using ArticleService.Services;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace ArticleService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TestRabbitMqController : ControllerBase
{
    private readonly IMessageClient _messageClient;

    public TestRabbitMqController(IMessageClient messageClient)
    {
        _messageClient = messageClient;
    }

    [HttpPost("publish")]
    public async Task<IActionResult> PublishTestArticle()
    {
        var evt = new ArticlePublishedEvent
        {
            Title = "Test Article via Controller",
            Content = "This is a test article from temporary controller",
            PublishedAt = DateTime.UtcNow,
            AuthorName = "Tester"
        };

        await _messageClient.Publish(evt);
        return Ok("Published test article!");
    }
}