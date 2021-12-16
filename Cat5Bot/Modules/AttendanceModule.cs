namespace Cat5Bot.Commands; //{}

[Group("attend"), Aliases("a"), Description("Used for attendance actions.")]
public class AttendModule : BaseCommandModule
{
    
}

[Group("unattend"), Aliases("ua"), Description("Used for attendance correction actions.")]
public class UnattendModule : BaseCommandModule
{

}