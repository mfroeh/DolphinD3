using System;
using System.Collections.Generic;

namespace Dolphin
{
    public class CacheServiceBase : ICacheService
    {
        private readonly IDictionary<string, object> cache = new Dictionary<string, object>();

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

        protected virtual string GetTypeBasedKey<TKey>(TKey key)
        {
            return $"typeof(key).Name_{key}";
        }
    }
}