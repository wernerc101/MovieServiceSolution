using System;
using System.Threading.Tasks;
using MovieService.Console.Commands;
using MovieService.Console.Interfaces;
using MovieService.Console.Services;
using SimpleInjector;

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
                        await container.GetInstance<SearchCommand>().ExecuteAsync();
                        break;
                    case "2":
                        await container.GetInstance<CreateCommand>().ExecuteAsync();
                        break;
                    case "3":
                        await container.GetInstance<QueryCommand>().ExecuteAsync();
                        break;
                    case "4":
                        await container.GetInstance<UpdateCommand>().ExecuteAsync();
                        break;
                    case "5":
                        await container.GetInstance<DeleteCommand>().ExecuteAsync();
                        break;
                    case "0":
                        return;
                    default:
                        System.Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}