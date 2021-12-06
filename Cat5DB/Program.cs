var builder = WebApplication.CreateBuilder(args);

// UNFRICK YOU SSL <3

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

Dictionary<string, string> paths = new();
paths.Add("root", "/");

/*
app.Use((ctx, next) =>
{
    string url = ctx.Request.Host + ctx.Request.Path;
    string host = ctx.Request.Host.ToString();
    bool validHost = host == "db.team3489.tk" || host == "localhost";
    if (!validHost || !paths.ContainsValue(ctx.Request.Path) || ctx.Request.Method != "GET") ctx.Abort();
    return next(ctx);
});
*/

MetaDatabase db = new($"{Directory.GetCurrentDirectory()}/Cat5.db");
Task dbTask = Task.Run(() => db.Start(1, 10000));

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

// lock in really old attendance
// fss
// google sheets, using team3489 email for github

await db.ExecuteAsync(db =>
{
    Console.WriteLine(db.TableExists("test"));
    Console.WriteLine(db.TableExists("teste"));
});

app.MapGet(paths["root"], async ctx =>
{
    await db.ExecuteAsync(db =>
    {
        db.EnsureTableExists("test");
    });
    //ctx.Response.Headers.Append("Access-Control-Allow-Origin", "*");
    await ctx.Response.WriteAsync("Hello, World!");
});

Task appTask = app.RunAsync();

Console.WriteLine("[Cat5DB] API running");
await appTask;
Console.WriteLine("[Cat5DB] API stopped, stopping database");
await db.StopAsync();
await dbTask;

Console.WriteLine("[Cat5DB] Shut down");