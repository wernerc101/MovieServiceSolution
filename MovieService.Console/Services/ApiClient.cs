using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MovieService.Console.Interfaces;
using MovieService.Common.Models;
using System.Collections.Generic;

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

        public async Task<bool> DeleteCachedEntryAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/cachedentries/{id}");
            return response.EnsureSuccessStatusCode().IsSuccessStatusCode;
        }

        public async Task<CachedEntryDto> GetCachedEntryAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CachedEntryDto>($"api/cachedentries/{id}");
        }

        public async Task<IEnumerable<CachedEntryDto>> GetAllCachedEntriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CachedEntryDto>>("api/cachedentries");
        }

        public async Task<bool> UpdateCachedEntryAsync(CachedEntryDto entry)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/cachedentries/{entry.Id}", entry);
            return response.EnsureSuccessStatusCode().IsSuccessStatusCode;
        }

        public async Task<IEnumerable<CachedEntryDto>> SearchCachedEntriesAsync(string searchTerm, string year, int? id)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CachedEntryDto>>($"api/cachedentries/search?term={searchTerm}");
        }
    }
}