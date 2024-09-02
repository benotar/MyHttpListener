using HttpLearning.Data.Interfaces;
using HttpLearning.Data.Storages;
using HttpLearning.Entities;
using Microsoft.EntityFrameworkCore;

namespace HttpLearning.Services;

public class TodoStorageDbContextService : ITodoStorage, IDisposable
{
    private readonly TodoStorageDbContext _db;

    public TodoStorageDbContextService()
    {
        _db = new TodoStorageDbContext();
    }
    
    public async Task<IEnumerable<Todo>> GetAll()
    {
        var todoes = await _db.Todoes.AsNoTracking().ToListAsync();

        return todoes;
    }

    public async Task Create(string title)
    {
        var newTodo = new Todo { Title = title };

        await _db.AddAsync(newTodo);

        await _db.SaveChangesAsync();
    }

    public async Task Delete(Guid? todoId)
    {
        var existingTodo = await _db.Todoes.FirstOrDefaultAsync(td => td.Id.Equals(todoId));

        if (existingTodo is null)
        {
            throw new ArgumentException($"Todo with id {todoId} not found!");
        }

        _db.Todoes.Remove(existingTodo);

        await _db.SaveChangesAsync();
    }

    public async Task Update(Guid? todoId, string title, bool isCompleted)
    {
        var existingTodo = await _db.Todoes.FirstOrDefaultAsync(td => td.Id.Equals(todoId));

        if (existingTodo is null)
        {
            throw new ArgumentException($"Todo with id {todoId} not found!");
        }

        existingTodo.Title = title;

        existingTodo.IsCompleted = isCompleted;

        existingTodo.UpdatedAt = DateTime.Now;

        _db.Update(existingTodo);

        await _db.SaveChangesAsync();
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}