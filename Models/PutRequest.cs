namespace HttpLearning.Models;

record PutRequest(Guid Id, string Title, bool IsCompleted);
