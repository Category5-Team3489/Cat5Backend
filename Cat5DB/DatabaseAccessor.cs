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

    #region Person
    public async Task<Cat5Person> CreatePerson(string name, byte permission)
    {
        Guid guid = Guid.NewGuid();
        Cat5Person cat5Person = null;
        await database.ExecuteAsync(db =>
        {
            ab.TryGetTable("people", out Table personTable);
            bool nameExists = false;
            foreach (Entry entry in personTable.table.Values)
            {
                StringEntry person = entry as StringEntry;
                if (person.value)
                {
                    nameExists = true;
                    break;
                }
            }
            if (!nameExists)
            {
                StringEntry person = new(name, guid.ToString());
                person.TryAddChild(new ByteEntry("permission", permission));
                person.TryAddChild(new Entry("attended"));
            }
        });
        return cat5Person;
    }
    public async Task<Cat5Person> GetPerson(string name)
    {
        bool exists = false;
        await database.ExecuteAsync(db =>
        {
            db.TryGetTable("people", out Table personTable);
            exists = personTable.TryGetEntry()
        });
    }
    #endregion Person

    #region Event
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
    public async Task<Cat5Event> GetEvent(Guid guid)
    {
        Cat5Event cat5Event = null;
        await database.ExecuteAsync(db =>
        {
            db.TryGetTable("events", out Table eventTable);
            if (eventTable.TryGetEntry(guid, out Entry entry))
            {
                StringEntry @event = entry as StringEntry;
                @event.TryGetChild("type", out Entry typeEntry);
                string type = (typeEntry as StringEntry).value;
                @event.TryGetChild("time", out Entry timeEntry);
                DateTime time = DateTime.FromFileTime((timeEntry as LongEntry).value);
                @event.TryGetChild("length", out Entry lengthEntry);
                TimeSpan length = new((lengthEntry as LongEntry).value);
                cat5Event = new Cat5Event(Guid.Parse(@event.Name), @event.value, type, time, length);
            }
        });
        return cat5Event;
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
    #endregion Event
}