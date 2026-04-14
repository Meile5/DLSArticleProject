using Microsoft.AspNetCore.Mvc;
using SubscriberService.Dtos;
using SubscriberService.Services;

namespace SubscriberService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SubscribersController : ControllerBase
{
    private readonly ISubscriberService _service;

    public SubscribersController(ISubscriberService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<SubscriberReadDto>> Subscribe([FromBody] SubscriberCreateDto dto)
    {
        var result = await _service.SubscribeAsync(dto);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Unsubscribe([FromQuery] string email)
    {
        var success = await _service.UnsubscribeAsync(email);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubscriberReadDto>>> GetSubscribers()
    {
        var subs = await _service.GetSubscribersAsync();
        return Ok(subs);
    }
}