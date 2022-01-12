namespace Cat5Bot;

public static class IO
{
    public readonly static HttpClient httpClient = new();
    public static string? ApiKey { get; set; }
}