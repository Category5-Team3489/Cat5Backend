string trusoKey = await File.ReadAllTextAsync(@"E:\Projects\Visual Studio\Cat5Backend\Cat5DB.Publish\truso.secret");

string publishPath = @"E:\Projects\Visual Studio\Cat5Backend\Cat5DB\bin\Release\net6.0\publish";

HttpClient client = new();
_ = await client.GetAsync(GetUri("delete", "file=Cat5DB/appsettings.json,Cat5DB/Cat5DB"));
using FileStream f1 = File.Open($@"{publishPath}\appsettings.json", FileMode.Open);
using GZipStream f1c = new(f1, CompressionMode.Compress);
_ = await client.PostAsync(GetUri("upload", "file=Cat5DB/appsettings.json"), new StreamContent(f1));
using FileStream f2 = File.Open($@"{publishPath}\Cat5DB", FileMode.Open);
using GZipStream f2c = new(f1, CompressionMode.Compress);
_ = await client.PostAsync(GetUri("upload", "file=Cat5DB/appsettings.json"), new StreamContent(f2));

string GetUri(string ep, string query)
{
    return $"https://db.team3489.tk:8443/{ep}?key={trusoKey}&{query}";
}