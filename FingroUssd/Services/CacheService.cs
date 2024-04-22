using FingroUssd.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace FingroUssd.Services;

public class CacheService: ICacheService
{
    private readonly IDatabase _db = LazyDatabase.Value;
    private readonly int _lifetime;

    public CacheService()
    {
        IConfiguration configuration = new ConfigurationManager().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
        _lifetime = configuration.GetValue<int>("Lifetime");
    }

    private static readonly Lazy<IDatabase> LazyDatabase = new(() =>
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json") // Adjust the configuration file name as needed
            .Build();

        var host = configuration.GetSection("Redis:Host").Get<string>();
        var port = configuration.GetSection("Redis:Port").Get<int>();
        var username = configuration.GetSection("Redis:Username").Get<string>();
        var password = configuration.GetSection("Redis:Password").Get<string>();

        var connectionMultiplexer = ConnectionMultiplexer.Connect($"{host}:{port},password={password}");
        return connectionMultiplexer.GetDatabase();
    });

    public T GetData<T>(string key)
    {
        var value = _db.StringGet(key);
        return !string.IsNullOrEmpty(value) ? JsonConvert.DeserializeObject<T>(value) : default;
    }

    public bool HasData<T>(string key)
    {
        return _db.KeyExists(key);
    }

    public void SetData<T>(string key, T value)
    {
        var lastKey = key.Split('-').Last();
        var endKey = key.Substring(0, key.Length - lastKey.Length) + "end";

        var end = GetData<string>(endKey);

        if (end != null)
        {
            end = DateTimeOffset.Now.AddSeconds(_lifetime).ToString();
            var ttl = Convert.ToDateTime(end) - DateTime.Now;
            _db.StringSet(key, JsonConvert.SerializeObject(value), ttl);
        }
        else
        {
            _db.StringSet(key, JsonConvert.SerializeObject(value), TimeSpan.FromSeconds(_lifetime));
        }
    }
    
    public void RemoveData(string key)
    {
        var isKeyExist = _db.KeyExists(key);
        if(isKeyExist) _db.KeyDelete(key);
    }
}