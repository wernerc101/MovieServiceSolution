using System;
using System.Threading.Tasks;
using MovieService.Console.Commands;
using MovieService.Console.Interfaces;
using MovieService.Console.Services;
using SimpleInjector;
using MovieService.Common.Models;

namespace MovieService.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var container = new Container();
            // Register services and commands
            container.Register<IApiClient, ApiClient>();
            container.Register<CreateCommand>();
            container.Register<DeleteCommand>();
            container.Register<QueryCommand>();
            container.Register<SearchCommand>();
            container.Register<UpdateCommand>();

            container.Verify();

            System.Console.WriteLine("Welcome to the Movie Service Console!");
            System.Console.WriteLine("Please choose an option:");
            System.Console.WriteLine("1. Search Cached Entries");
            System.Console.WriteLine("2. Create Cached Entry");
            System.Console.WriteLine("3. Query Cached Entries");
            System.Console.WriteLine("4. Update Cached Entry");
            System.Console.WriteLine("5. Delete Cached Entry");
            System.Console.WriteLine("0. Exit");

            while (true)
            {
                var choice = System.Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        var searchCommand = container.GetInstance<SearchCommand>();
                        await searchCommand.ExecuteAsync();
                        break;
                    case "2":
                        var createCommand = container.GetInstance<CreateCommand>();
                        // Create a new CachedEntryDto object to pass to the command
                        var newEntry = GetCachedEntryFromUser();
                        await createCommand.ExecuteAsync(newEntry);
                        break;
                    case "3":
                        var queryCommand = container.GetInstance<QueryCommand>();
                        await queryCommand.ExecuteAsync("","",1);
                        break;
                    case "4":
                        var updateCommand = container.GetInstance<UpdateCommand>();
                        System.Console.Write("Enter ID to update: ");
                        if (int.TryParse(System.Console.ReadLine(), out int updateId))
                        {
                            var updatedEntry = GetCachedEntryFromUser();
                            await updateCommand.ExecuteAsync(updateId, updatedEntry);
                        }
                        else
                        {
                            System.Console.WriteLine("Invalid ID format.");
                        }
                        break;
                    case "5":
                        var deleteCommand = container.GetInstance<DeleteCommand>();
                        System.Console.Write("Enter ID to delete: ");
                        if (int.TryParse(System.Console.ReadLine(), out int deleteId))
                        {
                            await deleteCommand.ExecuteAsync(deleteId);
                        }
                        else
                        {
                            System.Console.WriteLine("Invalid ID format.");
                        }
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private static CachedEntryDto GetCachedEntryFromUser()
        {
            var entry = new CachedEntryDto();
            
            System.Console.Write("Enter Title: ");
            entry.Title = System.Console.ReadLine();
            
            //System.Console.Write("Enter Description: ");
            //entry.Description = System.Console.ReadLine();
            
            // Add any other required fields
            
            return entry;
        }
    }
}