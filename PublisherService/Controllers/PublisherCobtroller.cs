using PublisherService.Entities;
using Microsoft.AspNetCore.Mvc;
namespace PublisherService.Controllers;


[ApiController]
[Route("api/publish")]
public class PublisherController(Services.PublisherService _service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Publish(PublishArticleRequest request)
    {
        await _service.PublishArticleAsync(request);
        return Ok();
    }
}