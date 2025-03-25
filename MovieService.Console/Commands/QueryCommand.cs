using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieService.Console.Interfaces;
using MovieService.Common.Models;

namespace MovieService.Console.Commands
{
    public class QueryCommand
    {
        private readonly IApiClient _apiClient;

        public QueryCommand(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task ExecuteAsync(string title, string year, int? id)
        {
            try
            {
                IEnumerable<CachedEntryDto> cachedEntries = await _apiClient.SearchCachedEntriesAsync(title, year, id);
                foreach (var entry in cachedEntries)
                {
                    System.Console.WriteLine($"ID: {entry.Id}, Title: {entry.Title}, Year: {entry.Year}");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred while retrieving cached entries: {ex.Message}");
            }
        }
    }
}