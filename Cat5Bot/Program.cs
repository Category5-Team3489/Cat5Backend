Console.WriteLine("Hello, World!");

string token = File.ReadAllText(Directory.GetCurrentDirectory() + "/token.secret");

var discord = new DiscordClient(new DiscordConfiguration()
{
    Token = token,
    TokenType = TokenType.Bot,
    Intents = DiscordIntents.AllUnprivileged
});

discord.UseInteractivity();

var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
{
    StringPrefixes = new[] { "!" }
});

commands.RegisterCommands<AttendModule>();
commands.RegisterCommands<UnattendModule>();
commands.RegisterCommands<NameModule>();

await discord.ConnectAsync();

while (true)
{
    if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.X)
        break;
    await Task.Delay(1000);
}