﻿using Microsoft.Extensions.Caching.Distributed;
using MuratBaloglu.Application.Abstractions.Services.Caching;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MuratBaloglu.Infrastructure.Services.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private static readonly ConcurrentDictionary<string, bool> CacheKeys = new ConcurrentDictionary<string, bool>();

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            string? cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);

            if (cachedValue is null)
                return null;

            T? value = JsonConvert.DeserializeObject<T>(cachedValue);

            return value;
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
        {
            T? cachedValue = await GetAsync<T>(key, cancellationToken);

            if (cachedValue is not null)
            {
                return cachedValue;
            }

            cachedValue = await factory();

            await SetAsync(key, cachedValue, cancellationToken);

            return cachedValue;
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
        {
            string cacheValue = JsonConvert.SerializeObject(value);

            await _distributedCache.SetStringAsync(key, cacheValue, cancellationToken);

            CacheKeys.TryAdd(key, false); //For RemoveByPrefixAsync Method
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);

            CacheKeys.TryRemove(key, out bool _); //For RemoveByPrefixAsync Method
        }

        public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
        {
            //foreach (string key in CacheKeys.Keys)
            //{
            //    if (key.StartsWith(prefixKey))
            //    {
            //        await RemoveAsync(key, cancellationToken);
            //    }
            //}

            //Daha performanslı alternatif yol
            IEnumerable<Task> tasks = CacheKeys.Keys
                .Where(k => k.StartsWith(prefixKey))
                .Select(k => RemoveAsync(k, cancellationToken));

            await Task.WhenAll(tasks);
        }
    }
}
