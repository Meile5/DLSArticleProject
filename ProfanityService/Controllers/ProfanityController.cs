using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProfanityService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfanityController : ControllerBase
{
    //private Database database = Database.GetInstance();
    
    [HttpDelete]
    public void Delete()
    {
        //database.DeleteDatabase();
    }

    [HttpPost]
    public void Post()
    {
        //database.RecreateDatabase();
    }
}