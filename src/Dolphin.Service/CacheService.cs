using Dolphin.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dolphin.Service
{
    public class CacheService : ICacheService
    {
        private readonly IDictionary<string, object> cache = new Dictionary<string, object>();
        private readonly ILogService logService;

        public CacheService(ILogService logService)
        {
            this.logService = logService;
        }

        public async Task Add<TKey, TValue>(TKey key, TValue value)
        {
            var _key = GetTypeBasedKey(key);
            await Add(_key, value);
        }

        public async Task Add<TValue>(string key, TValue value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new Exception("Key must not be null or whitespace");
            if (value == null) throw new Exception("Wont cache null value");

            if (!cache.ContainsKey(key))
                cache[key] = value;
        }

        public async Task<TValue> Get<TKey, TValue>(TKey key)
        {
            var _key = GetTypeBasedKey(key);
            return await Get<TValue>(_key);
        }

        public async Task<TValue> Get<TValue>(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new Exception("Key must not be null or whitespace");

            if (cache.ContainsKey(key))
            {
                var value = cache[key];

                if (value != null)
                    return (TValue)value;
                else
                    cache.Remove(key);
            }

            return default(TValue);
        }

        public async Task<IDictionary<string, object>> GetCache()
        {
            return cache;
        }

        private string GetTypeBasedKey<TKey>(TKey key)
        {
            if (key is BuffName buffName)
                return $"BuffName_{buffName}";
            else if (key is SkillName skillName)
                return $"SkillName_{skillName}";
            else if (key is WorldLocation location)
                return $"WorldLocation_{location}";

            return "";
        }
    }
}