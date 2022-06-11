using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MoviesLibrary.Services
{
    internal class MemoryCacheService
    {
        private readonly ObjectCache _cache;
        private readonly CacheItemPolicy _policy;

        public ObjectCache ObjectCache => _cache;

        public MemoryCacheService(DateTimeOffset absoluteExpiration = default)
        {
            _cache = MemoryCache.Default;
            _policy = new CacheItemPolicy()
            {
                AbsoluteExpiration = absoluteExpiration
            };
        }

        /// <summary>
        /// 添加或更新缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Set<T>(string key, T value) where T : class, new()
        {
            ObjectCache.Set(key, value, _policy);
        }

        /// <summary>
        /// 添加或更新缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="absoluteExpiration">过期时间</param>
        /// <param name="regionName"></param>
        public void Set<T>(string key, T value, DateTimeOffset absoluteExpiration, string? regionName = null)
        {
            ObjectCache.Set(key, value, absoluteExpiration, regionName);
        }


        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="regionName"></param>
        /// <returns>T</returns>
        public T Get<T>(string key, string? regionName = null) where T : class, new()
        {
            return ObjectCache.Get(key, regionName) as T ?? new T();
        }
    }
}
