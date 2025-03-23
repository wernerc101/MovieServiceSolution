namespace MovieService.Api.Interfaces
{
    public interface ICacheService
    {
        void AddCachedEntry(CachedEntryDto entry);
        CachedEntryDto GetCachedEntry(string id);
        IEnumerable<CachedEntryDto> GetAllCachedEntries();
        void UpdateCachedEntry(CachedEntryDto entry);
        void DeleteCachedEntry(string id);
        void ClearCache();
    }
}