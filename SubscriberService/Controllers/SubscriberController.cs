using SubscriberService.Dtos;
using SubscriberService.Services;
namespace SubscriberService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

[ApiController]
[Route("api/v1/[controller]")]
public class SubscribersController : ControllerBase
{
    private readonly ISubscriberService _service;
    private readonly IFeatureManager _featureManager;

    public SubscribersController(ISubscriberService service, IFeatureManager featureManager)
    {
        _service = service;
        _featureManager = featureManager;
    }

    [HttpPost]
    public async Task<ActionResult<SubscriberReadDto>> Subscribe([FromBody] SubscriberCreateDto dto)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.SubscriberService))
            return StatusCode(503, "SubscriberService is currently disabled.");

        var result = await _service.SubscribeAsync(dto);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Unsubscribe([FromQuery] string email)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.SubscriberService))
            return StatusCode(503, "SubscriberService is currently disabled.");

        var success = await _service.UnsubscribeAsync(email);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubscriberReadDto>>> GetSubscribers()
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.SubscriberService))
            return StatusCode(503, "SubscriberService is currently disabled.");

        var subs = await _service.GetSubscribersAsync();
        return Ok(subs);
    }
}