namespace Cat5DB;

public static class ValidationHelpers
{
    public static bool FileTimeValid(long fileTime)
    {
        return fileTime >= DateTime.MinValue.Ticks && fileTime <= DateTime.MaxValue.Ticks;
    }
}