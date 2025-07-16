using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;

namespace Xrm
{
    public class DataverseCache
    {
        private static readonly DataverseCache _instance = new DataverseCache();
        private static readonly MemoryCache Cache = new MemoryCache(typeof(DataverseCache).FullName);
        private static readonly ConcurrentDictionary<string, object> LocksByKey = new ConcurrentDictionary<string, object>();
        
        public static DataverseCache Instance => _instance;
        private static DateTime GetDefaultExpirationTime => DateTime.UtcNow.AddHours(2);

        public T GetOrAdd<T>(string key, Func<T> getValue)
        {
            return GetOrAdd(key, getValue, GetDefaultExpirationTime);
        }
        
        public T GetOrAdd<T>(string key, Func<T> getValue, DateTime expiration)
        {
            var value = (T)Cache.Get(key);

            if (value != null)
            {
                return value;
            }

            var lockForKey = LocksByKey.GetOrAdd(key, k => new object());

            lock (lockForKey)
            {
                value = (T)Cache.Get(key);

                if (value != null)
                {
                    return value;
                }

                value = getValue();

                Cache.Set(key, value, new CacheItemPolicy
                {
                    AbsoluteExpiration = new DateTimeOffset(expiration)
                });
            }

            return value;
        }
    }
}