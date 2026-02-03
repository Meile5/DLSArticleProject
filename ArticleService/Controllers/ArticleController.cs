using Microsoft.AspNetCore.Mvc;

namespace ArticleService.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticleController : ControllerBase
{
    private Database database = Database.GetInstance();
    
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