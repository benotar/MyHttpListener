using HttpLearning.Data.Interfaces;
using HttpLearning.Entities;

namespace HttpLearning.Data.Storages;

public class ListTodoStorage : ITodoStorage
{
    private readonly List<Todo> _todoes = new()
    {
        new Todo { Title = "First Test Todo" },
        new Todo { Title = "Second Test Todo" },
    };

    private Todo? _existingTodo = null;

    public async Task<IEnumerable<Todo>> GetAll()
    {
        return await Task.Run(() => { return _todoes; });
    }

    public async Task Create(string title)
    {
        await Task.Run(() => { _todoes.Add(new Todo { Title = title }); });
    }

    public async Task Delete(Guid? todoId)
    {
        await Task.Run(() =>
        {
            _existingTodo = _todoes.FirstOrDefault(td => td.Id.Equals(todoId));

            if (_existingTodo is null)
            {
                throw new ArgumentException($"Todo with id {todoId} not found!");
            }

            _todoes.Remove(_existingTodo);
        });
    }

    public async Task Update(Guid? todoId, string title, bool isCompleted)
    {
        await Task.Run(() =>
        {
            _existingTodo = _todoes.FirstOrDefault(td => td.Id.Equals(todoId));

            if (_existingTodo is null)
            {
                throw new ArgumentException($"Todo with id {todoId} not found!");
            }

            _existingTodo.Title = title;

            _existingTodo.IsCompleted = isCompleted;

            _existingTodo.UpdatedAt = DateTime.Now;
        });
    }

    // Unused
    public void Dispose()
    {
        // TODO release managed resources here
    }
}