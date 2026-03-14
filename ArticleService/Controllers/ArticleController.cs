using ArticleService.Dtos;
using ArticleService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticleService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _service;
    private readonly ArticleCacheService _cacheService;
    public ArticlesController(IArticleService service, ArticleCacheService articleCacheService)
    {
        _service = service;
        _cacheService = articleCacheService;
    }

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

    [HttpGet("{articleId}")]
    public async Task<ActionResult<ArticleReadDto>> GetArticleById(Guid articleId)
    {
        var article = await _service.GetArticleByIdAsync(articleId);
        if (article == null)
            return NotFound();
        return Ok(article);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleReadDto>>> GetAllArticles()
    {
        var articles = await _service.GetAllArticlesAsync();
        return Ok(articles);
    }

    [HttpPut("{articleId}")]
    public async Task<IActionResult> UpdateArticle(Guid articleId, [FromBody] ArticleUpdateDto dto)
    {
        var updated = await _service.UpdateArticleAsync(articleId, dto);
        if (!updated)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{articleId}")]
    public async Task<IActionResult> DeleteArticle(Guid articleId)
    {
        var deleted = await _service.DeleteArticleAsync(articleId);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
    
    [HttpGet("recent")]
    public IActionResult GetRecentArticles()
    {
        var articles = _cacheService.GetArticles();
        return Ok(articles);
    }
}