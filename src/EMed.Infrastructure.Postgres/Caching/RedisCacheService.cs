using System.Text.Json;
using EMed.Application.Interfaces.Caching;
using StackExchange.Redis;

namespace EMed.Infrastructure.Postgres.Caching;

public class RedisCacheService : ICacheService, ICacheInvalidator
{
    private const string Prefix = "emed:";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IDatabase _database;
    private readonly IConnectionMultiplexer _redis;
    
    public RedisCacheService(IConnectionMultiplexer redis)
    { 
        _redis = redis;
        _database = _redis.GetDatabase();
    }
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var value = await _database.StringGetAsync(String.Concat(Prefix, key));

        if (value.IsNullOrEmpty)
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(value.ToString(), JsonOptions );
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var serialized = JsonSerializer.Serialize(value, JsonOptions );
        
        await _database.StringSetAsync(String.Concat(Prefix, key), serialized, expiration);
  
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        await _database.KeyDeleteAsync(String.Concat(Prefix, key)); 
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        foreach (var endpoint in _redis.GetEndPoints())
        {
            var server = _redis.GetServer(endpoint);

            if (server.IsReplica)
                continue;
            
            var pattern = $"{Prefix}{prefix}*";
            
            await foreach (var key in server.KeysAsync(pattern: pattern, pageSize: 500).WithCancellation(cancellationToken))
            {
                await _database.KeyDeleteAsync(key);
            }
        }
    }
}