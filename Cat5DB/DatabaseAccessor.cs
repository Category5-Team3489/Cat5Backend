namespace Cat5DB;

public class DatabaseAccessor
{
    private MetaDatabase db;

    public DatabaseAccessor(MetaDatabase db)
    {
        this.db = db;
    }

    public async Task Init()
    {
        await db.ExecuteAsync(db =>
        {
            db.EnsureTableExists("events");
            db.EnsureTableExists("names");
            db.EnsureTableExists("attendance");
            db.EnsureTableExists("permissions");
            if (db.TryGetTable("events", out Table events))
            {
                events.TryAddEntry(new Entry("12/8/21 Meeting"));
            }
        });
    }

    // Event Entry: Name: EventId: Guid
        // StringEntry: EventName
        // StringEntry: EventType
        // LongEntry: EventTime: DateTime: Local File Time
        // LongEntry: EventLength: TimeSpan: Ticks


    public async Task EventExists(string name)
    {
        if (db.TryGetTable())
    }

    public async Task<Entry> CreateEvent()
    {
        Guid guid = Guid.NewGuid();
    }
}