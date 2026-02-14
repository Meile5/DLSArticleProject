using ArticleService.Database;
using Microsoft.AspNetCore.Mvc;

namespace ArticleService.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticleController : ControllerBase
{
    private ArticleDatabase database = ArticleDatabase.GetInstance();
    
    [HttpGet]
    public Dictionary<int, int> Get()
    {
        Console.WriteLine("called GET request");
        return  database.GetAllArticles();
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