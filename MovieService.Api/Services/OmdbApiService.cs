using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MovieService.Api.DTO;
using MovieService.Api.Interfaces;

namespace MovieService.Api.Services
{
    public class OmdbApiService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OmdbApiService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<MovieSearchResponse> SearchMoviesAsync(MovieSearchRequest request)
        {
            var response = await _httpClient.GetAsync($"?s={request.Title}&y={request.Year}&apikey={_apiKey}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<MovieSearchResponse>();
        }

        public async Task<MovieDetail> GetMovieDetailsAsync(string id)
        {
            var response = await _httpClient.GetAsync($"?i={id}&apikey={_apiKey}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<MovieDetail>();
        }
    }
}