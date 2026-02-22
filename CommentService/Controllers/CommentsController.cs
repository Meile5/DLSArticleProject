using Microsoft.AspNetCore.Mvc;
using ProfanityService.Models.Dtos;

namespace CommentService.Controllers;


[ApiController]
[Route("[controller]")]
public class CommentsController(Service.CommentsService commentService) : ControllerBase
{
    [HttpPost]
    public async Task <ActionResult > SaveComment(CommentDto commentDto)
    {
        throw NotImplementedException;
    }

    public Exception NotImplementedException { get; set; }
}