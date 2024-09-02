using HttpLearning.Data.Interfaces;
using HttpLearning.Data.Storages;
using HttpLearning.Services;

namespace HttpLearning.Providers;

public static class TodoStorageProvider
{
    public static ITodoStorage? CreateTodoStorage(int userChoice)
        => userChoice switch
        {
            1 => new ListTodoStorage(),
            2 => new TodoStorageDbContextService(),
            _ => null
        };
}