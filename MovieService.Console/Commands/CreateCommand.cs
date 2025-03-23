using System;
using System.Threading.Tasks;
using MovieService.Console.Interfaces;
using MovieService.Common.Models;

namespace MovieService.Console.Commands
{
    public class CreateCommand
    {
        private readonly IApiClient _apiClient;

        public CreateCommand(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task ExecuteAsync(CachedEntryDto cachedEntryDto)
        {
            if (cachedEntryDto == null)
            {
                throw new ArgumentNullException(nameof(cachedEntryDto));
            }

            var result = await _apiClient.CreateCachedEntryAsync(cachedEntryDto);
            Console.WriteLine($"Cached entry created with ID: {result.Id}");
        }
    }
}