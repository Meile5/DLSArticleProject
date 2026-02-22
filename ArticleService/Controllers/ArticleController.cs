using ArticleService.Dtos;
using ArticleService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticleService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _service;

        public ArticlesController(IArticleService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ArticleReadDto>> CreateArticle([FromBody] ArticleCreateDto dto)
        {
            var created = await _service.CreateArticleAsync(dto);
            return CreatedAtAction(nameof(GetArticleById), new { articleId = created.ArticleId }, created);
        }

        [HttpGet("{articleId}")]
        public async Task<ActionResult<ArticleReadDto>> GetArticleById(Guid articleId)
        {
            var article = await _service.GetArticleByIdAsync(articleId);
            if (article == null) return NotFound();
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
            try
            {
                await _service.UpdateArticleAsync(articleId, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{articleId}")]
        public async Task<IActionResult> DeleteArticle(Guid articleId)
        {
            await _service.DeleteArticleAsync(articleId);
            return NoContent();
        }
    }
}