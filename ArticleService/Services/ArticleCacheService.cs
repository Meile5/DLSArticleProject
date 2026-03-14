namespace ArticleService.Services;

using Entities;
using Microsoft.Extensions.Caching.Memory;

public class ArticleCacheService
{
    private readonly IMemoryCache _cache;
    private const string CacheKey = "ArticleCache";

    public ArticleCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void SetArticles(IEnumerable<Article> articles)
    {
        _cache.Set(CacheKey, articles, TimeSpan.FromMinutes(15)); // auto-expire in 15 mins
    }

    public IEnumerable<Article> GetArticles()
    {
        return _cache.Get<IEnumerable<Article>>(CacheKey) ?? Enumerable.Empty<Article>();
    }
}