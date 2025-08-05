using System;
using System.Collections.Concurrent;
using System.Linq;
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

        /// <summary>
        /// Get or add an item to the cache with a default expiration time of 2 hours.
        /// </summary>
        /// <param name="key">Key for cached item</param>
        /// <param name="getValue">Function for getting the item if it isn't in the cache</param>
        /// <typeparam name="T">Type of the cached item</typeparam>
        /// <returns>Item</returns>
        public T GetOrAdd<T>(string key, Func<T> getValue)
        {
            return GetOrAdd(key, getValue, GetDefaultExpirationTime);
        }

        /// <summary>
        /// Get or add an item to the cache with a specified expiration time.
        /// </summary>
        /// <typeparam name="T">Type of the cached item</typeparam>
        /// <param name="key">Key for the cached item</param>
        /// <param name="getValue">Function for getting the item if it isn't in the cache</param>
        /// <param name="expiration">Expiration time of the cached item</param>
        /// <returns>Item</returns>Cached item<summary>
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
                value = getValue();

                Cache.Set(key, value, new CacheItemPolicy
                {
                    AbsoluteExpiration = new DateTimeOffset(expiration)
                });
            }

            return value;
        }

        /// <summary>
        /// Remove an item from the cache. Uses a Contains filter to find all items with a similar key
        /// </summary>
        /// <param name="key">Key of cache items to remove</param>
        public void Remove(string key)
        {
            var keys = Cache.Where(c => c.Key.Contains(key)).Select(c => c.Key);

            foreach (var cacheKey in keys)
            {
                var lockForKey = LocksByKey.GetOrAdd(cacheKey, k => new object());

                lock (lockForKey)
                {
                    if (Cache.Contains(cacheKey))
                    {
                        Cache.Remove(cacheKey);
                        LocksByKey.TryRemove(cacheKey, out _);
                    }
                }
            }
        }

    }
}