using ArticleService.Dtos;
using ArticleService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticleService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _service;

    public ArticlesController(IArticleService service)
    {
        _service = service;
    }

    // CREATE
    [HttpPost]
    public async Task<ActionResult<ArticleReadDto>> CreateArticle([FromBody] ArticleCreateDto dto)
    {
        var created = await _service.CreateArticleAsync(dto);
        return CreatedAtAction(
            nameof(GetArticleById),
            new { articleId = created.ArticleId },
            created
        );
    }

    // GET by ID
    [HttpGet("{articleId}")]
    public async Task<ActionResult<ArticleReadDto>> GetArticleById(Guid articleId)
    {
        var article = await _service.GetArticleByIdAsync(articleId);
        if (article == null)
            return NotFound();
        return Ok(article);
    }

    // GET all
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleReadDto>>> GetAllArticles()
    {
        var articles = await _service.GetAllArticlesAsync();
        return Ok(articles);
    }

    // UPDATE
    [HttpPut("{articleId}")]
    public async Task<IActionResult> UpdateArticle(Guid articleId, [FromBody] ArticleUpdateDto dto)
    {
        var updated = await _service.UpdateArticleAsync(articleId, dto);
        if (!updated)
            return NotFound();
        return NoContent();
    }

    // DELETE
    [HttpDelete("{articleId}")]
    public async Task<IActionResult> DeleteArticle(Guid articleId)
    {
        var deleted = await _service.DeleteArticleAsync(articleId);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}