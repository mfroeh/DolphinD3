using System.Collections.Generic;

namespace Dolphin
{
    public interface ICacheService<TKey, TCacheValue>
    {
        IDictionary<TKey, TCacheValue> GetCache();

        void AddToCache(TKey key, TCacheValue value);

        TCacheValue GetCachedValue(TKey key);
    }
}