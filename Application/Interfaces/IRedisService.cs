using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRedisService
    {
        public Task<T?> Get<T>(string key, CancellationToken cancellationToken = default) where T : class;


        public  Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default) where T : class;


        public Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default);

        public Task<bool> RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default);


        public Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
        
    }
}
