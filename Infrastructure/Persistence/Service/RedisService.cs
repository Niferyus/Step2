using Application.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Service
{
    public class RedisService : IRedisService
    {
        private readonly string _host;
        private readonly int _port;
        private ConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;

        public RedisService(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public void Connect()
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");
            _database = _connectionMultiplexer.GetDatabase(0);
        }

        public IDatabase GetDatabase() => _connectionMultiplexer.GetDatabase(0);

        public async Task<T?> Get<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            var value = await _database.StringGetAsync(key);
            if (!value.HasValue)
                return null;

            return JsonSerializer.Deserialize<T>(value!);
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default) where T : class
        {
            var serializedValue = JsonSerializer.Serialize(value);
            return await _database.StringSetAsync(key, serializedValue, expiry);
        }

        public async Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<bool> RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
        {
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            var keys = server.Keys(pattern: pattern).ToArray();

            if (keys.Length == 0)
                return true;

            return await _database.KeyDeleteAsync(keys) > 0;
        }

        public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
        {
            return await _database.KeyExistsAsync(key);
        }

        public void Dispose()
        {
            _connectionMultiplexer?.Dispose();
        }
    }
}
