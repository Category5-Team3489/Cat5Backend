var builder = WebApplication.CreateBuilder(args);

// UNFRICK YOU SSL <3

// lock in really old attendance
// fss
// google sheets, using team3489 email for github

// https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("https://db.team3489.tk", "https://localhost").AllowAnyHeader().AllowAnyMethod();
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

// DB
{
    // Attendance Table
    {
        // Events Entry
        {

        }
        // Names Entry
        {
            // List
            {
                // NameDBObject
                {

                }
            }
        }
        // Attendance Entry
        {

        }
        // Permissions Entry
        {

        }
    }
}

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

app.MapGet("/create", async ctx =>
{
    var e = await dba.CreateEvent("12/8/21 Meeting", "Meeting", new DateTime(2021, 12, 8, 17, 30, 0), new TimeSpan(2, 0, 0));
    await ctx.Response.WriteAsJsonAsync(e);
});

app.MapGet("/events", async ctx =>
{
    DateTime start = DateTime.MinValue;
    DateTime end = DateTime.MaxValue;
    try
    {
        if (ctx.Request.Query.ContainsKey("start") && long.TryParse(ctx.Request.Query["start"], out long startFileTime))
            if (startFileTime >= DateTime.MinValue.Ticks && startFileTime <= DateTime.MaxValue.Ticks)
                start = DateTime.FromFileTime(startFileTime);
        if (ctx.Request.Query.ContainsKey("end"))
            if (long.TryParse(ctx.Request.Query["end"], out long endFileTime))
                end = DateTime.FromFileTime(endFileTime);
        List<Cat5Event> events = await dba.GetEvents(start, end);
        await ctx.Response.WriteAsJsonAsync(events);
    }
    catch (Exception)
    {
        await ctx.Response.WriteAsync("404");
    }
});

Task appTask = app.RunAsync();
Console.WriteLine("[Cat5DB] API running");
await appTask;
Console.WriteLine("[Cat5DB] API stopped, stopping database");
await database.StopAsync();
await dbTask;
Console.WriteLine("[Cat5DB] Shut down");