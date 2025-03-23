using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using MovieService.Api.Interfaces;
using MovieService.Api.Models;

namespace MovieService.Api.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void AddCachedEntry(CachedEntry cachedEntry)
        {
            _memoryCache.Set(cachedEntry.Id, cachedEntry);
        }

        public CachedEntry GetCachedEntry(Guid id)
        {
            _memoryCache.TryGetValue(id, out CachedEntry cachedEntry);
            return cachedEntry;
        }

        public IEnumerable<CachedEntry> GetAllCachedEntries()
        {
            return _memoryCache.GetKeys<CachedEntry>().Select(key => _memoryCache.Get<CachedEntry>(key));
        }

        public void RemoveCachedEntry(Guid id)
        {
            _memoryCache.Remove(id);
        }

        public void UpdateCachedEntry(CachedEntry cachedEntry)
        {
            _memoryCache.Set(cachedEntry.Id, cachedEntry);
        }
    }
}