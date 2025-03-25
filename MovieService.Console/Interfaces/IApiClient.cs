using MovieService.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieService.Console.Interfaces
{
    public interface IApiClient
    {
        Task<IEnumerable<CachedEntryDto>> SearchCachedEntriesAsync(string title = null, string year = null, int? id = null);
        Task<CachedEntryDto> GetCachedEntryAsync(int id);
        Task<CachedEntryDto> CreateCachedEntryAsync(CachedEntryDto entry);
        Task<bool> UpdateCachedEntryAsync(CachedEntryDto entry);
        Task<bool> DeleteCachedEntryAsync(int id);
    }
}