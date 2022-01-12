namespace Cat5Backend.Shared;

//https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/

public record Cat5Event(Guid Guid, string Name, string Type, DateTime Time, TimeSpan Length);

public record Cat5Person(Guid Guid, string Name, byte Permission, ulong DiscordId, List<Guid> Attended);