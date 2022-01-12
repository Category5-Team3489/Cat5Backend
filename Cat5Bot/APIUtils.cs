namespace Cat5Bot;

public static class APIUtils
{
    public const string ApiEndpoint = "https://db.team3489.tk/";

    // possible problem: HttpClient may not put %20 in request URI

    public static async Task<Cat5Person> CreatePerson(string name, ulong discordId)
    {
        string json = await IO.httpClient.GetStringAsync(GetURI("createperson", $"name={name}&discordId={discordId}"));
        return JsonSerializer.Deserialize<Cat5Person>(json)!;
    }
    public static async Task<List<Cat5Person>> People()
    {
        string json =  await IO.httpClient.GetStringAsync(GetURI("people", $""));
        return JsonSerializer.Deserialize<List<Cat5Person>>(json)!;
    }
    public static async Task<string> CreateEvent(string name, string type, long time, long length)
    {
        return await IO.httpClient.GetStringAsync(GetURI("createevent", $"name={name}&type={type}&"));
    }
    public static async Task<string> People()
    {
        return await IO.httpClient.GetStringAsync(GetURI("people", $""));
    }

    public static string GetURI(string feature, string data)
    {
        return $"{ApiEndpoint}{feature}?key={IO.ApiKey}&{data}";
    }
}