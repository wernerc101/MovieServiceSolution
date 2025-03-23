using System;
using System.Threading.Tasks;
using MovieService.Console.Interfaces;

namespace MovieService.Console.Commands
{
    public class DeleteCommand
    {
        private readonly IApiClient _apiClient;

        public DeleteCommand(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task ExecuteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Console.WriteLine("Invalid ID. Please provide a valid cached entry ID.");
                return;
            }

            var success = await _apiClient.DeleteCachedEntryAsync(id);
            if (success)
            {
                Console.WriteLine($"Cached entry with ID {id} has been deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to delete cached entry with ID {id}. It may not exist.");
            }
        }
    }
}