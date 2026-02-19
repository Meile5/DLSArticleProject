using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using ProfanityService.Models.Dtos;

namespace ProfanityService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfanityController(Service.ProfanityService profanityService) : ControllerBase
{
    [HttpPost]
    public async Task <ActionResult <bool>> CheckForForbiddenWords(CommentDto commentDto)
    {
        return await profanityService.CheckForbiddenWords(commentDto);
    }
}