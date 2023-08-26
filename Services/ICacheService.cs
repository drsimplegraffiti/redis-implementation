

namespace CachingRedis.Services;
    public interface ICacheService
    {
        T GetData<T>(string key); // T is the type of data we want to get from cache
        bool SetData<T>(string key, T value, DateTimeOffset expirationTime); // T is the type of data we want to set in cache
        object RemoveData(string key); 
    }