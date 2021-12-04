var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.Urls.Add("http://*:80");

MetaDatabase db = new($"{Directory.GetCurrentDirectory()}/Cat5.db");
List<Task> tasks = new();
tasks.Add(Task.Run(() => db.Start(1, 10000)));

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

app.MapGet("/", async ctx =>
{
    await db.ExecuteAsync(db =>
    {
        db.EnsureTableExists("test");
    });
    ctx.Response.Headers.Append("Access-Control-Allow-Origin", "*");
    await ctx.Response.WriteAsync("Hello, World!");
});

tasks.Add(app.RunAsync());
await Task.WhenAll(tasks);