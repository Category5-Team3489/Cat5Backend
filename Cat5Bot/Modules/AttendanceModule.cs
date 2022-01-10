namespace Cat5Bot.Commands; //{}

[Group("attend"), Aliases("a"), Description("Used for attendance actions.")]
public class AttendModule : BaseCommandModule
{
    [GroupCommand, Description("Marks your attendance for an event occuring today.")]
    public async Task AttendSelf(CommandContext ctx)
    {
        await ctx.RespondAsync($"Testing!");
    }
}

/*
[Group("unattend"), Aliases("ua"), Description("Used for attendance correction actions.")]
public class UnattendModule : BaseCommandModule
{
    [GroupCommand, Description("Marks your attendance for an event occuring today.")]
    public async Task UnattendSelf(CommandContext ctx)
    {
        await ctx.RespondAsync($"Testing");
    }
}
*/