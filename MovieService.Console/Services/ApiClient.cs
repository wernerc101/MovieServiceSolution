using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MovieService.Console.Interfaces;
using MovieService.Common.Models;

namespace MovieService.Console.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CachedEntryDto> CreateCachedEntryAsync(CachedEntryDto entry)
        {
            var response = await _httpClient.PostAsJsonAsync("api/cachedentries", entry);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CachedEntryDto>();
        }

        public async Task DeleteCachedEntryAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/cachedentries/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<CachedEntryDto> GetCachedEntryAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CachedEntryDto>($"api/cachedentries/{id}");
        }

        public async Task<IEnumerable<CachedEntryDto>> GetAllCachedEntriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CachedEntryDto>>("api/cachedentries");
        }

        public async Task<CachedEntryDto> UpdateCachedEntryAsync(CachedEntryDto entry)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/cachedentries/{entry.Id}", entry);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CachedEntryDto>();
        }

        public async Task<IEnumerable<CachedEntryDto>> SearchCachedEntriesAsync(string searchTerm)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CachedEntryDto>>($"api/cachedentries/search?term={searchTerm}");
        }
    }
}