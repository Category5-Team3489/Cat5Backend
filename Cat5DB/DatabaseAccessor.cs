namespace Cat5DB;

public class DatabaseAccessor
{
    private MetaDatabase database;

    public DatabaseAccessor(MetaDatabase database)
    {
        this.database = database;
    }

    public async Task Init()
    {
        await database.ExecuteAsync(db =>
        {
            db.EnsureTableExists("events");
            db.EnsureTableExists("people");
        });
    }

    // Event StringEntry: Name: EventId: Guid, Value: Event Name
        // StringEntry: EventType
        // LongEntry: EventTime: DateTime: Local File Time
        // LongEntry: EventLength: TimeSpan: Ticks
    // Person StringEntry: Name: PersonId: Guid, Value: Full Name
        // Permission Level ByteEntry: Name: PermissionLevel, Value: Permission Level
        // Attended Events Entry: Name: AttendedEvents
            // Attended Event Entry: Name: EventId: Guid

    /*
    public async Task<> CreatePerson(string personId, byte permissionLevel)
    {
        
    }
    */


    public async Task<bool> EventExists(string name)
    {
        bool exists = false;
        await database.ExecuteAsync(db =>
        {
            db.TryGetTable("events", out Table eventTable);
            exists = eventTable.TryGetEntry(name, out _);
        });
        return exists;
    }

    public async Task<List<Cat5Event>> GetEvents(DateTime start, DateTime end)
    {
        List<Cat5Event> events = new();
        await database.ExecuteAsync(db =>
        {
            if (db.TryGetTable("events", out Table eventTable))
            {
                foreach (Entry entry in eventTable.table.Values)
                {
                    StringEntry @event = entry as StringEntry;

                    @event.TryGetChild("type", out Entry typeEntry);
                    string type = (typeEntry as StringEntry).value;

                    @event.TryGetChild("time", out Entry timeEntry);
                    DateTime time = DateTime.FromFileTime((timeEntry as LongEntry).value);

                    if (time < start) continue;

                    @event.TryGetChild("length", out Entry lengthEntry);
                    TimeSpan length = new((lengthEntry as LongEntry).value);

                    if (time.Add(length) > end) continue;

                    events.Add(new Cat5Event(Guid.Parse(@event.Name), @event.value, type, time, length));
                }
            }
        });
        return events;
    }

    public async Task<Cat5Event> CreateEvent(string name, string type, DateTime time, TimeSpan length)
    {
        Guid guid = Guid.NewGuid();
        await database.ExecuteAsync(db =>
        {
            db.TryGetTable("events", out Table eventTable);
            StringEntry @event = new(guid.ToString(), name);
            @event.TryAddChild(new StringEntry("type", type));
            @event.TryAddChild(new LongEntry("time", time.ToFileTime()));
            @event.TryAddChild(new LongEntry("length", length.Ticks));
            eventTable.TryAddEntry(@event);
        });
        return new Cat5Event(guid, name, type, time, length);
    }
}