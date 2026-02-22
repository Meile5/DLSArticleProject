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
        await commentService.SaveComment(createCommentDto);
        return Ok();
    }
    
    
    [HttpGet]
    [Route("Get-Comments")]
    public async Task<ActionResult<CommentsListDto>> GetComments([FromQuery] ArticleDto articleDto)
    {
        var result = await commentService.GetComments(articleDto);
        if (result.Comments.Count == 0)
        {
            return NotFound();
        }
        return Ok(result);
    }
    
}