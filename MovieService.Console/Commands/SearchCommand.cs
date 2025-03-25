using System;
using System.Linq;
using System.Threading.Tasks;
using MovieService.Console.Interfaces;
using MovieService.Console.Services;

namespace MovieService.Console.Commands
{
    public class SearchCommand
    {
        private readonly IApiClient _apiClient;

        public SearchCommand(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task ExecuteAsync(string title = null, string year = null, int? id = null)
        {
            var results = await _apiClient.SearchCachedEntriesAsync(title, year, id);
            if (results != null && results.Any())
            {
                foreach (var entry in results)
                {
                    System.Console.WriteLine($"ID: {entry.Id}, Title: {entry.Title}, Year: {entry.Year}");
                }
            }
            else
            {
                System.Console.WriteLine("No entries found.");
            }
        }
    }
}