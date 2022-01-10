namespace Cat5Bot;

public static class IO
{
    public readonly static HttpClient httpClient = new();
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static string? apiKey;
#pragma warning restore CA2211 // Non-constant fields should not be visible
}