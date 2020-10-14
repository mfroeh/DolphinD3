using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dolphin
{
    public interface ICacheService
    {
        Task<IDictionary<string, object>> GetCache();

        Task Add<TKey, TValue>(TKey key, TValue value);

        Task Add<TValue>(string key, TValue value);

        Task<TValue> Get<TKey, TValue>(TKey key);

        Task<TValue> Get<TValue>(string key);
    }
}