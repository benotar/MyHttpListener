using System.Text.Json;

namespace HttpLearning.Providers;

public static class JsonProvider
{
    private static JsonSerializerOptions _options;

    static JsonProvider()
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public static string Serialize<T>(T obj)
        => JsonSerializer.Serialize(obj, _options);

    public static T Deserialize<T>(string obj)
        => JsonSerializer.Deserialize<T>(obj, _options);
}