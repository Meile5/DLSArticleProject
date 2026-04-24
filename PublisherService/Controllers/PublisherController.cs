using PublisherService.Entities;
using Microsoft.AspNetCore.Mvc;
using MonitorService;
using Serilog;

namespace PublisherService.Controllers;


[ApiController]
[Route("api/publish")]
public class PublisherController(Services.PublisherService _service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Publish(PublishArticleRequest request)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "development")
        {
            using var activity = Monitoring.ActivitySource
                .StartActivity("Publish method called in PublisherController (POST request)");
        
            Log.Logger.Debug("Publish method called in PublisherController (POST request)");
        }
        
        await _service.PublishArticleAsync(request);
        return Ok();
    }
}