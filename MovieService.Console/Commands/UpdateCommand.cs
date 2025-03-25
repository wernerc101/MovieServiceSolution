using System;
using System.Threading.Tasks;
using MovieService.Console.Interfaces;
using MovieService.Common.Models;

namespace MovieService.Console.Commands
{
    public class UpdateCommand
    {
        private readonly IApiClient _apiClient;

        public UpdateCommand(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task ExecuteAsync(int id, CachedEntryDto updatedEntry)
        {
            if (id <= 0)
            {
                System.Console.WriteLine("Invalid entry ID.");
                return;
            }

            var result = await _apiClient.UpdateCachedEntryAsync(updatedEntry);
            if (result)
            {
                System.Console.WriteLine("Cached entry updated successfully.");
            }
            else
            {
                System.Console.WriteLine("Failed to update cached entry.");
            }
        }
    }
}