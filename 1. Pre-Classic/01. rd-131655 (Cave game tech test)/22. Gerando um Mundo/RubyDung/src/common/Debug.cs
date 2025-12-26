public class Debug
{
    public static void Log(object? mensage, bool showMilliseconds = false)
    {
        WriteFormated(mensage, null, showMilliseconds);
    }

    public static void LogWarning(object? mensage, bool showMilliseconds = false)
    {
        WriteFormated(mensage, ConsoleColor.Yellow, showMilliseconds);
    }

    public static void LogError(object? mensage, bool showMilliseconds = false)
    {
        WriteFormated(mensage, ConsoleColor.Red, showMilliseconds);
    }

    public static void LogSuccess(object? mensage, bool showMilliseconds = false)
    {
        WriteFormated(mensage, ConsoleColor.Green, showMilliseconds);
    }
    
    public static void LogInfo(object? mensage, bool showMilliseconds = false)
    {
        WriteFormated(mensage, ConsoleColor.Cyan, showMilliseconds);
    }

    private static void WriteFormated(object? value, ConsoleColor? color = null, bool showMilliseconds = false)
    {
        string format = showMilliseconds ? "[HH:mm:ss.fff] " : "[HH:mm:ss] ";
        Console.Write(DateTime.Now.ToString(format));

        Console.ForegroundColor = color.HasValue ? color.Value : Console.ForegroundColor;
        Console.WriteLine(value);
        Console.ResetColor();
    }
}
