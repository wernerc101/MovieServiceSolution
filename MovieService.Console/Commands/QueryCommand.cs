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

        public async Task ExecuteAsync()
        {
            try
            {
                IEnumerable<CachedEntryDto> cachedEntries = await _apiClient.GetCachedEntriesAsync();
                foreach (var entry in cachedEntries)
                {
                    Console.WriteLine($"ID: {entry.Id}, Title: {entry.Title}, Year: {entry.Year}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving cached entries: {ex.Message}");
            }
        }
    }
}