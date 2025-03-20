using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;
using Microsoft.Xrm.Sdk;

namespace Xrm
{
    public class DataverseCache
    {
        private static readonly DataverseCache _instance = new DataverseCache();
        private static readonly MemoryCache Cache = new MemoryCache(typeof(DataverseCache).FullName);
        private static readonly ConcurrentDictionary<string, object> LocksByKey = new ConcurrentDictionary<string, object>();
        
        public static DataverseCache Instance => _instance;
        private static DateTime GetDefaultExpirationTime => DateTime.UtcNow.AddHours(2);

        public static T GetOrAdd<T>(string key, Func<string, T> getValue)
        {
            return GetOrAdd(key, getValue, GetDefaultExpirationTime);
        }
        
        public static T GetOrAdd<T>(string key, Func<string, T> getValue, DateTime expiration)
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

                value = getValue(key);

                Cache.Set(key, value, new CacheItemPolicy
                {
                    AbsoluteExpiration = new DateTimeOffset(expiration)
                });
            }

            return value;
        }
    }
}