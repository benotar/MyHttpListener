using HttpLearning.Entities;

namespace HttpLearning.Data.Interfaces;

public interface ITodoStorage : IDisposable
{
    Task<IEnumerable<Todo>> GetAll();

    Task Create(string title);

    Task Delete(Guid? todoId);

    Task Update(Guid? todoId, string title, bool isCompleted);
}