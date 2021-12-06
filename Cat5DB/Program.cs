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

MetaDatabase db = new($"{Directory.GetCurrentDirectory()}/Cat5.db");
Task dbTask = Task.Run(() => db.Start(1, 1000));

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

DatabaseAccessor dba = new(db);

await dba.Init();

app.MapGet("/", async ctx =>
{
    await ctx.Response.WriteAsync("Hello, World!");
});



Task appTask = app.RunAsync();
Console.WriteLine("[Cat5DB] API running");
await appTask;
Console.WriteLine("[Cat5DB] API stopped, stopping database");
await db.StopAsync();
await dbTask;
Console.WriteLine("[Cat5DB] Shut down");