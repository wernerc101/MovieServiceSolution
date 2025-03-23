using System;
using System.Threading.Tasks;
using MovieService.Console.Commands;
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

            Console.WriteLine("Welcome to the Movie Service Console!");
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. Search Cached Entries");
            Console.WriteLine("2. Create Cached Entry");
            Console.WriteLine("3. Query Cached Entries");
            Console.WriteLine("4. Update Cached Entry");
            Console.WriteLine("5. Delete Cached Entry");
            Console.WriteLine("0. Exit");

            while (true)
            {
                var choice = Console.ReadLine();
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
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}