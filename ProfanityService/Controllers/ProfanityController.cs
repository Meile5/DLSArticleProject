using Microsoft.AspNetCore.Mvc;
using MonitorService;
using ProfanityService.Models.Dtos;
using Serilog;

namespace ProfanityService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfanityController(Service.ProfanityService profanityService) : ControllerBase
{
    [HttpPost]
    public async Task <ActionResult <bool>> CheckForForbiddenWords(CommentDto commentDto)
    {
        using var activity = Monitoring.ActivitySource.StartActivity("Entered CheckForForbiddenWords in ProfanityController (POST Request)");
        
        Log.Logger.Debug("Entered CheckForForbiddenWords in ProfanityController (POST Request)");
        return await profanityService.CheckForbiddenWords(commentDto);
    }
}