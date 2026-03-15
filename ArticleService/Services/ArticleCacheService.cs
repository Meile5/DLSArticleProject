using Prometheus;

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
        var articles = _cache.Get<IEnumerable<Article>>(CacheKey);
        if (articles == null)
        {
            CacheMisses.Inc(); return Enumerable.Empty<Article>();
        } 
        CacheHits.Inc();                                                                                                                                  
        return articles;
    }
    
    private static readonly Counter CacheHits = Metrics
        .CreateCounter("articlecache_hits_total", "Total number of article cache hits");                                                                                                                                
    private static readonly Counter CacheMisses = Metrics
        .CreateCounter("articlecache_misses_total", "Total number of article cache misses");
}