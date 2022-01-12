var builder = WebApplication.CreateBuilder();

Console.WriteLine(Guid.NewGuid().ToString());

// UNFRICK YOU SSL <3

// lock in really old attendance
// fss
// google sheets, using team3489 email for github

// figured out how to do website login, use a query string in url that client routes

// https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0

// https://developers.google.com/sheets/api/quickstart/dotnet

// could use channels instead of concurrent queue for synced actions in database



// def need more levels of perms, currently anyone with bot api key could do unintended things

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("https://localhost", "https://team3489.tk").AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseCors("AllowSpecificOrigins");

MetaDatabase database = new($"{Directory.GetCurrentDirectory()}/Cat5.db");
Task dbTask = Task.Run(() => database.Start(1, 1000));

string apiKeysPath = $"{Directory.GetCurrentDirectory()}/apikeys.secret";
List<string> apiKeys;
if (File.Exists(apiKeysPath)) apiKeys = new(File.ReadAllLines(apiKeysPath));
else apiKeys = new();

IReadOnlyList<string> publicEndpoints = new List<string> { "/guid" };

app.Use(async (ctx, next) =>
{
    if (ctx.Request.Host.ToString() != "db.team3489.tk" && !app.Environment.IsDevelopment())
    {
        ctx.Response.StatusCode = 403;
        await ctx.Response.WriteAsync("403");
        return;
    }
    if (!publicEndpoints.Contains(ctx.Request.Path.ToString()))
    {
        if (!ctx.Request.Query.ContainsKey("key"))
        {
            ctx.Response.StatusCode = 400;
            await ctx.Response.WriteAsync("400");
            return;
        }
        if (!apiKeys.Contains(ctx.Request.Query["key"]))
        {
            ctx.Response.StatusCode = 401;
            await ctx.Response.WriteAsync("401");
            return;
        }
    }
    await next(ctx);
});

DatabaseAccessor dba = new(database);
await dba.Init();

app.MapGet("/", async ctx =>
{
    Cat5Event a = new(Guid.NewGuid(), "12/8/21 Meeting", "Meeting", new DateTime(2021, 12, 8, 17, 30, 0), new TimeSpan(2, 0, 0));
    string t = JsonSerializer.Serialize(a);
    Cat5Event b = JsonSerializer.Deserialize<Cat5Event>(t);
    Console.WriteLine(t);
    //ctx.Request.ReadFromJsonAsync(Cat5Event);
    await ctx.Response.WriteAsync(t);
    //await ctx.Response.WriteAsync("Hello, World!");
});

app.MapGet("/createperson", async ctx =>
{
    if (!ctx.Request.Query.ContainsKey("name"))
        return;
    string name = ctx.Request.Query["name"];
    if (!ctx.Request.Query.ContainsKey("discordId") || !ulong.TryParse(ctx.Request.Query["discordId"], out ulong discordId))
        return;


    var p = await dba.CreatePerson(name, 0, discordId);
    await ctx.Response.WriteAsJsonAsync(p);
});

app.MapGet("/people", async ctx =>
{
    var p = await dba.GetPeople();
    await ctx.Response.WriteAsJsonAsync(p);
});

app.MapGet("/attendevent", async ctx =>
{
    if (!ctx.Request.Query.ContainsKey("personId")) return;
    if (!ctx.Request.Query.ContainsKey("eventId")) return;
    if (!Guid.TryParse(ctx.Request.Query["personId"], out Guid personId))
        return;
    if (!Guid.TryParse(ctx.Request.Query["eventId"], out Guid eventId))
        return;
    var p = await dba.AttendEvent(personId, eventId);
    await ctx.Response.WriteAsJsonAsync(p);
});

app.MapGet("/createevent", async ctx =>
{
    if (!ctx.Request.Query.ContainsKey("name")) return;
    if (!ctx.Request.Query.ContainsKey("type")) return;
    if (!ctx.Request.Query.ContainsKey("time")) return;
    if (!ctx.Request.Query.ContainsKey("length")) return;

    if (!long.TryParse(ctx.Request.Query["time"], out long timeFileTime) || !ValidationHelpers.FileTimeValid(timeFileTime))
        return;
    if (!long.TryParse(ctx.Request.Query["length"], out long lengthTicks))
        return;

    var e = await dba.CreateEvent(ctx.Request.Query["name"], ctx.Request.Query["type"], DateTime.FromFileTime(timeFileTime), TimeSpan.FromTicks(lengthTicks));
    await ctx.Response.WriteAsJsonAsync(e);
});

app.MapGet("/events", async ctx =>
{
    DateTime start = DateTime.MinValue;
    DateTime end = DateTime.MaxValue;
    if (ctx.Request.Query.ContainsKey("start") && long.TryParse(ctx.Request.Query["start"], out long startFileTime) && ValidationHelpers.FileTimeValid(startFileTime))
        start = DateTime.FromFileTime(startFileTime);
    if (ctx.Request.Query.ContainsKey("end") && long.TryParse(ctx.Request.Query["end"], out long endFileTime) && ValidationHelpers.FileTimeValid(endFileTime))
        end = DateTime.FromFileTime(endFileTime);
    List<Cat5Event> events = await dba.GetEvents(start, end);
    await ctx.Response.WriteAsJsonAsync(events);
});

app.MapGet("/guid", async ctx =>
{
    await ctx.Response.WriteAsync(Guid.NewGuid().ToString());
});

Task appTask = app.RunAsync();
Console.WriteLine("[Cat5DB] API running");
await appTask;
Console.WriteLine("[Cat5DB] API stopped, stopping database");
await database.StopAsync();
await dbTask;
Console.WriteLine("[Cat5DB] Shut down");