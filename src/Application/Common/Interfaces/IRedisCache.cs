namespace Application.Common.Interfaces;

public interface IRedisCache
{
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task<T?> GetAsync<T>(string key);
}