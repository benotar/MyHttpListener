using HttpLearning;
using HttpLearning.Providers;

await Console.Out.WriteLineAsync("1 - Memory storage.\n2 - Database Storage.");

await Console.Out.WriteAsync("Your choice - ");

if (!int.TryParse(await Console.In.ReadLineAsync(), out var userChoice))
{
    await Console.Out.WriteLineAsync("An error occurred during the operation of the program!");

    return;
}

var todoStorage = TodoStorageProvider.CreateTodoStorage(userChoice);

if (todoStorage is null)
{
    await Console.Out.WriteLineAsync($"Invalid choice - {userChoice}!\nAccess denied!");
    
    return;
}

var server = new TodoHttpListener(5555);

var serverConfig = new Startup(server, todoStorage);

serverConfig.ConfigureServerEvents();

await server.ListenAsync();