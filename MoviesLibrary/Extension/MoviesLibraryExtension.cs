using Microsoft.Extensions.DependencyInjection;
using MoviesLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesLibrary.Extension
{
    public static class MoviesLibraryExtension
    {
        /// <summary>
        /// 影视服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="enableMemoryCache">是否启用缓存，默认启用</param>
        /// <param name="absoluteExpiration">缓存时间，默认一个小时</param>
        /// <returns></returns>
        public static IServiceCollection AddMoviesLibrary(this IServiceCollection services, bool enableMemoryCache = true, DateTimeOffset absoluteExpiration = default)
        {
            MovieService movieService = new(enableMemoryCache, absoluteExpiration);
            services.AddSingleton(movieService);
            return services;
        }
    }
}
