using Application.Common.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infrastructure.Redis;

public class RedisCache : IRedisCache
{
    private readonly IDatabase _database;

    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        Converters = { new GuidConverter() },
        Formatting = Formatting.None,
        NullValueHandling = NullValueHandling.Ignore
    };

    public RedisCache(string connectionString)
    {
        var redisConnection = ConnectionMultiplexer.Connect(connectionString);
        _database = redisConnection.GetDatabase();
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var serializedValue = JsonConvert.SerializeObject(value, JsonSettings);
        await _database.StringSetAsync(key, serializedValue, expiry);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var serializedValue = await _database.StringGetAsync(key);

        if (string.IsNullOrEmpty(serializedValue))
            return default;

        try
        {
            var deserializedValue = JsonConvert.DeserializeObject<T>(serializedValue!, JsonSettings);
            return deserializedValue;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Deserialization failed for key '{key}': {ex.Message}");
            return default;
        }
    }

    private class GuidConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Guid) || objectType == typeof(Guid?);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is Guid guid)
            {
                writer.WriteValue(guid.ToString());
            }
            else
            {
                writer.WriteNull();
            }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return objectType == typeof(Guid?) ? null : Guid.Empty;

            var stringValue = reader.Value?.ToString();
            return string.IsNullOrEmpty(stringValue)
                ? (objectType == typeof(Guid?) ? null : Guid.Empty)
                : Guid.TryParse(stringValue, out var guid)
                    ? guid
                    : Guid.Empty;
        }
    }
}