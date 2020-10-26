using System.Collections.Generic;
using System.Drawing;

namespace Dolphin
{
    public interface ICacheService
    {
        void Add<TKey, TValue>(TKey key, TValue value);

        void Add<TValue>(string key, TValue value);

        TValue Get<TKey, TValue>(TKey key);

        TValue Get<TValue>(string key);

        IDictionary<string, object> GetCache();
    }
}