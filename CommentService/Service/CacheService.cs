using System.Text.Json;
using StackExchange.Redis;

namespace CommentService.Service;

public class CacheService
{
    private readonly IDatabase _db;
    private const string LRUKey = "commentcache:lru";

    public CacheService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task SetAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json);
        await UpdateLRUAsync(key);
    }
    
    private async Task UpdateLRUAsync(string articleId)
    {
        var count = await _db.ExecuteAsync("DBSIZE");
        long keyCount = (long)count;
        
        if (keyCount >= 30)
        {
            await _db.ListRemoveAsync(LRUKey, articleId);
            var oldest = await _db.ListLeftPopAsync(LRUKey);
            if (!oldest.IsNullOrEmpty)
            {
                await _db.KeyDeleteAsync(oldest.ToString());
            }
        }
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        if (value.IsNullOrEmpty) return default;
        await UpdateLRUAsync(key);
        return JsonSerializer.Deserialize<T>(value!.ToString());
    }
    
}