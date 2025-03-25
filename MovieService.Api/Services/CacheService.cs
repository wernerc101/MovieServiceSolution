using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using MovieService.Api.Interfaces;
using MovieService.Common.Models;

namespace MovieService.Api.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private const string CacheKeyPrefix = "CachedEntry_";
        private const string AllEntriesKey = "AllCachedEntries";

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public void AddCachedEntry(CachedEntryDto entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            // Add/update the individual entry in cache
            string cacheKey = GetCacheKey(entry.Id.ToString());
            _memoryCache.Set(cacheKey, entry, TimeSpan.FromHours(1));

            // Update the all entries collection
            var allEntries = GetAllEntriesFromCache();
            if (allEntries.Any(e => e.Id == entry.Id))
            {
                var existingEntry = allEntries.First(e => e.Id == entry.Id);
                int index = allEntries.IndexOf(existingEntry);
                allEntries[index] = entry;
            }
            else
            {
                allEntries.Add(entry);
            }

            _memoryCache.Set(AllEntriesKey, allEntries, TimeSpan.FromHours(1));
        }

        public void ClearCache()
        {
            var allEntries = GetAllEntriesFromCache();
            
            // Remove each individual entry
            foreach (var entry in allEntries)
            {
                string cacheKey = GetCacheKey(entry.Id.ToString());
                _memoryCache.Remove(cacheKey);
            }
            
            // Remove the all entries collection
            _memoryCache.Remove(AllEntriesKey);
        }

        public void DeleteCachedEntry(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            // Remove the individual entry
            string cacheKey = GetCacheKey(id);
            _memoryCache.Remove(cacheKey);

            // Update the all entries collection
            var allEntries = GetAllEntriesFromCache();
            var entryToRemove = allEntries.FirstOrDefault(e => e.Id.ToString() == id);
            if (entryToRemove != null)
            {
                allEntries.Remove(entryToRemove);
                _memoryCache.Set(AllEntriesKey, allEntries, TimeSpan.FromHours(1));
            }
        }

        public IEnumerable<CachedEntryDto> GetAllCachedEntries()
        {
            return GetAllEntriesFromCache();
        }

        public CachedEntryDto GetCachedEntry(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            string cacheKey = GetCacheKey(id);
            if (_memoryCache.TryGetValue(cacheKey, out CachedEntryDto entry))
            {
                return entry;
            }

            // Try to find it in the all entries collection as a fallback
            var allEntries = GetAllEntriesFromCache();
            entry = allEntries.FirstOrDefault(e => e.Id.ToString() == id);
            
            return entry;
        }

        public void UpdateCachedEntry(CachedEntryDto entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            // This is essentially the same as adding since we're overwriting the cached value
            AddCachedEntry(entry);
        }

        // Helper methods
        private string GetCacheKey(string id)
        {
            return $"{CacheKeyPrefix}{id}";
        }

        private List<CachedEntryDto> GetAllEntriesFromCache()
        {
            if (!_memoryCache.TryGetValue(AllEntriesKey, out List<CachedEntryDto> allEntries))
            {
                allEntries = new List<CachedEntryDto>();
            }
            return allEntries;
        }
    }
}