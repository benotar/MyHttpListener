using System.Net;
using System.Text;
using HttpLearning.Data.Interfaces;
using HttpLearning.Data.Storages;
using HttpLearning.Models;
using HttpLearning.Providers;

namespace HttpLearning;

public class Startup
{
    private TodoHttpListener _server;

    private ITodoStorage _todoStorage;

    public Startup(TodoHttpListener server, ITodoStorage todoStorage)
    {
        _server = server;

        _todoStorage = todoStorage;
    }

    public void ConfigureServerEvents()
    {
        ConfigureOnGet();
        ConfigureOnPost();
        ConfigureOnPut();
        ConfigureOnDelete();
    }

    private void ConfigureOnGet()
    {
        _server.OnGet += async (request, response) =>
        {
            await HandleRequestAsync(response, async () =>
                JsonProvider.Serialize(await _todoStorage.GetAll()));
        };
    }

    private void ConfigureOnPost()
    {
        _server.OnPost += async (request, response) =>
        {
            await HandleRequestAsync(response, async () =>
            {
                var buffer = new byte[request.ContentLength64];

                await request.InputStream.ReadAsync(buffer); // CancellationToken = default (CancellationToken.None)

                var todoTitle = Encoding.UTF8.GetString(buffer);

                await _todoStorage.Create(todoTitle);

                return "New Todo successfully created!";
            });
        };
    }

    private void ConfigureOnPut()
    {
        _server.OnPut += async (request, response) =>
        {
            await HandleRequestAsync(response, async () =>
            {
                var buffer = new byte[request.ContentLength64];

                await request.InputStream.ReadAsync(buffer); // CancellationToken = default (CancellationToken.None)

                var putRequest = JsonProvider.Deserialize<PutRequest>(Encoding.UTF8.GetString(buffer));

                await _todoStorage.Update(putRequest.Id, putRequest.Title, putRequest.IsCompleted);

                return $"Todo with id \'{putRequest.Id}\' updated successfully!";
            });
        };
    }

    private void ConfigureOnDelete()
    {
        _server.OnDelete += async (request, response) =>
        {
            await HandleRequestAsync(response, async () =>
            {
                var buffer = new byte[request.ContentLength64];

                await request.InputStream.ReadAsync(buffer);

                var todoId = JsonProvider.Deserialize<Guid?>(Encoding.UTF8.GetString(buffer));

                if (!todoId.HasValue)
                {
                    throw new ArgumentException("Invalid input data!");
                }

                await _todoStorage.Delete(todoId);

                return "Deleted successfully!";
            });
        };
    }

    private static async Task HandleRequestAsync(HttpListenerResponse response, Func<Task<string>> processRequest)
    {
        byte[]? buffer = null;

        try
        {
            var responseBody = await processRequest();

            buffer = Encoding.UTF8.GetBytes(responseBody);

            response.StatusCode = (int)HttpStatusCode.OK;
        }
        catch (ArgumentException ex)
        {
            response.StatusCode = (int)HttpStatusCode.Conflict;

            buffer = Encoding.UTF8.GetBytes(ex.Message);
        }
        catch (Exception ex)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;

            buffer = Encoding.UTF8.GetBytes(ex.Message);
        }
        finally
        {
            response.ContentLength64 = buffer!.Length;

            await response.OutputStream.WriteAsync(buffer); // CancellationToken = default (CancellationToken.None)

            response.OutputStream.Close();
        }
    }

    public void EnsureEfCoreDatabaseUsed()
    {
        if (_todoStorage is TodoStorageDbContext)
        {
            _todoStorage.Dispose();
        }
    }
}