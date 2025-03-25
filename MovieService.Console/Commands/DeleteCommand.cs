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

        public async Task ExecuteAsync(int id)
        {
            var success = await _apiClient.DeleteCachedEntryAsync(id);
            if (success)
            {
                System.Console.WriteLine($"Cached entry with ID {id} has been deleted successfully.");
            }
            else
            {
                System.Console.WriteLine($"Failed to delete cached entry with ID {id}. It may not exist.");
            }
        }
    }
}