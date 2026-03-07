using DraftService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DraftService.Controllers;
[ApiController]
[Route("api/drafts")]
public class DraftController(Service.DraftService _service) : ControllerBase

{
    [HttpPost]
    public async Task<IActionResult> CreateDraft(DraftDto request)
    {
        await _service.CreateDraft(request);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetDraft(Guid id)
    {
        var draft = await _service.GetDraft(id);
        return Ok(draft);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDraft(Guid id, DraftDto request)
    {
        await _service.UpdateDraft(id, request);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDraft(Guid id)
    {
        await _service.DeleteDraft(id);
        return NoContent();
    }
}