using CommentService.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using CommentService.Service;

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
        using var activity = MonitorService.MonitorService.ActivitySource.StartActivity();
        
        //for Serilog debugging
        MonitorService.MonitorService.Log.Debug("Entered SaveComment in CommentsController");

        
        await commentService.SaveComment(createCommentDto);
        return Ok();
    }
    
    
    [HttpGet]
    [Route("Get-Comments")]
    public async Task<ActionResult<CommentsListDto>> GetComments([FromQuery] ArticleDto articleDto)
    {
        //for OpenTelemetry/Zipkin
        using var activity = MonitorService.MonitorService.ActivitySource.StartActivity();
        
        //for Serilog debugging
        MonitorService.MonitorService.Log.Debug("Entered GetComments in CommentsController");

        var result = await commentService.GetComments(articleDto);
        if (result.Comments.Count == 0)
        {
            return NotFound();
        }
        return Ok(result);
    }
    
}