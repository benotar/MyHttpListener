using System.Text.Json;
using HttpLearning.Providers;

namespace HttpLearning.Entities;

public class Todo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }

    public bool IsCompleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    public override string ToString()
    {
        return JsonProvider.Serialize(this);
    }
}