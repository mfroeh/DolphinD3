using Dolphin.Enum;
using System;
using System.Collections.Generic;

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

        public void Add<TKey, TValue>(TKey key, TValue value)
        {
            var _key = GetTypeBasedKey(key);
            Add(_key, value);
        }

        public void Add<TValue>(string key, TValue value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new Exception("Key must not be null or whitespace");
            if (value == null) throw new Exception("Wont cache null value");

            if (!cache.ContainsKey(key))
            {
                cache[key] = value;
            }
        }

        public TValue Get<TKey, TValue>(TKey key)
        {
            var _key = GetTypeBasedKey(key);
            return Get<TValue>(_key);
        }

        public TValue Get<TValue>(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new Exception("Key must not be null or whitespace");

            if (cache.ContainsKey(key))
            {
                var value = cache[key];

                if (value != null)
                {
                    return (TValue)value;
                }
                else
                {
                    cache.Remove(key);
                }
            }

            return default;
        }

        public IDictionary<string, object> GetCache()
        {
            return cache;
        }

        private string GetTypeBasedKey<TKey>(TKey key)
        {
            switch (key)
            {
                case BuffName buffName:
                    return $"BuffName_{buffName}";

                case SkillName skillName:
                    return $"SkillName_{skillName}";

                case WorldLocation location:
                    return $"WorldLocation_{location}";

                case PlayerClass playerClass:
                    return $"PlayerClass_{playerClass}";

                case PlayerHealth health:
                    return $"PlayerHealth_{health}";

                case Enum.PlayerResource resource:
                    return $"PlayerResource_{resource}";

                default:
                    throw new NotImplementedException($"Didnt implement GetTypeBasedKey for type {key.GetType().Name} in CacheService yet.");
            }
        }
    }
}