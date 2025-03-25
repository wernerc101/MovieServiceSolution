using MovieService.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieService.Console.Interfaces
{
    public interface IApiClient
    {
        Task<CachedEntryDto> CreateCachedEntryAsync(CachedEntryDto cachedEntry);
        Task<(CachedEntryDto, bool)> UpdateCachedEntryAsync(CachedEntryDto cachedEntry);
        Task<bool> DeleteCachedEntryAsync(int id);
        Task<CachedEntryDto> GetCachedEntryAsync(int id);
        Task<IEnumerable<CachedEntryDto>> SearchCachedEntriesAsync(string title, string year, int? id);
    }
}