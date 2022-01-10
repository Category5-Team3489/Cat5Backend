namespace Cat5Bot.Modules; //{}

[Group("name"), Aliases("n"), Description("Links a user's name to their account.")]
public class NameModule : BaseCommandModule
{
    [GroupCommand, Description("Links your full name to your attendance record.")]
    public async Task NameSelf(CommandContext ctx, [Description("Your full name")] params string[] fullName)
    {
        string msg = await IO.httpClient.GetStringAsync("https://api.coinbase.com/v2/prices/spot?currency=USD");
        await ctx.RespondAsync(msg);
    }

    [GroupCommand, Description("Links a full name to an attendance record.")]
    public async Task NameOther(CommandContext ctx, [Description("User")] DiscordUser user, [Description("Their full name")] params string[] fullName)
    {
        
    }
}