using CommentService.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using CommentService.Service;
using MonitorService;
using Serilog;

namespace CommentService.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CommentsController(CommentsService commentService) : ControllerBase
{
    [HttpPost]
    [Route("Create-Comment")]
    public async Task<IActionResult> SaveComment(CreateCommentDto createCommentDto)
    {
        //for OpenTelemetry/Zipkin
        using var activity = Monitoring.ActivitySource.StartActivity("Entered SaveComment in CommentsController (POST Request)");
        
        //for Serilog debugging
        Log.Logger.Debug("Entered SaveComment in CommentsController (POST request)");

        
        await commentService.SaveComment(createCommentDto);
        return Ok();
    }
    
    
    [HttpGet]
    [Route("Get-Comments")]
    public async Task<ActionResult<CommentsListDto>> GetComments([FromQuery] ArticleDto articleDto)
    {
        //for OpenTelemetry/Zipkin
        using var activity = Monitoring.ActivitySource.StartActivity("Entered GetComments in CommentsController (GET Request)");
        
        //for Serilog debugging
        Log.Logger.Debug("Entered GetComments in CommentsController (GET Request)");

        var result = await commentService.GetComments(articleDto);
        if (result.Comments.Count == 0)
        {
            return NotFound();
        }
        return Ok(result);
    }
    
}