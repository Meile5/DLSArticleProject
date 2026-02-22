using ArticleService.Dtos;
using ArticleService.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ArticleService.Controllers
{
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
        public async Task<ActionResult<ArticleReadDto>> CreateArticle(
            [FromBody] ArticleCreateDto dto,
            [FromQuery] string shard = "Global") // optional shard selection
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _service.CreateArticleAsync(dto, shard);
                return CreatedAtAction(
                    nameof(GetArticleById),
                    new { articleId = created.ArticleId, shard },
                    created
                );
            }
            catch (Exception ex)
            {
                // Log ex if using a logger
                return StatusCode(500, "An unexpected error occurred while creating the article.");
            }
        }

        // GET BY ID
        [HttpGet("{articleId}")]
        public async Task<ActionResult<ArticleReadDto>> GetArticleById(
            Guid articleId,
            [FromQuery] string shard = "Global")
        {
            try
            {
                var article = await _service.GetArticleByIdAsync(articleId, shard);
                if (article == null) return NotFound();
                return Ok(article);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving the article.");
            }
        }

        // GET ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleReadDto>>> GetAllArticles(
            [FromQuery] string shard = "Global")
        {
            try
            {
                var articles = await _service.GetAllArticlesAsync(shard);
                return Ok(articles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving articles.");
            }
        }

        // UPDATE
        [HttpPut("{articleId}")]
        public async Task<IActionResult> UpdateArticle(
            Guid articleId,
            [FromBody] ArticleUpdateDto dto,
            [FromQuery] string shard = "Global")
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _service.UpdateArticleAsync(articleId, dto, shard);
                if (!updated)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while updating the article.");
            }
        }

        // DELETE
        [HttpDelete("{articleId}")]
        public async Task<IActionResult> DeleteArticle(
            Guid articleId,
            [FromQuery] string shard = "Global")
        {
            try
            {
                var deleted = await _service.DeleteArticleAsync(articleId, shard);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while deleting the article.");
            }
        }
    }
}
