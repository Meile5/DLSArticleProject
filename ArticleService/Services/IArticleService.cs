using ArticleService.Dtos;

namespace ArticleService.Services;

    public interface IArticleService
    {
        Task<ArticleReadDto> CreateArticleAsync(ArticleCreateDto dto);
        Task<ArticleReadDto?> GetArticleByIdAsync(Guid articleId);
        Task<IEnumerable<ArticleReadDto>> GetAllArticlesAsync();
        Task UpdateArticleAsync(Guid articleId, ArticleUpdateDto dto);
        Task DeleteArticleAsync(Guid articleId);
    }
