using ArticleService.Database;
using Microsoft.AspNetCore.Mvc;

namespace ArticleService.Controllers;

[ApiController]
[Route("Article")]
public class ArticleController : ControllerBase
{
    private ArticleDatabase database = ArticleDatabase.GetInstance();
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        Console.WriteLine("called GET request");
        return Ok(database.GetAllArticles());
    }
    
    [HttpDelete]
    public void Delete()
    {
        database.DeleteDatabase();
    }

    [HttpPost]
    public void Post()
    {
        database.RecreateDatabase();
    }
}