namespace Cat5Bot.Commands; //{}

[Group("name"), Aliases("n"), Description("Used for linking a user's name to their account.")]
public class NameModule : BaseCommandModule
{
    [GroupCommand, Description("For linking your full name to your attendance record.")]
    public async Task NameSelf(CommandContext ctx, [Description("Your full name")] params string[] fullName)
    {
        
    }

    [GroupCommand, Description("For linking a full name to an attendance record.")]
    public async Task NameOther(CommandContext ctx, [Description("User")] DiscordUser user, [Description("Their full name")] params string[] fullName)
    {

    }
}