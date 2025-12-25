using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace RubyDung;

public class Program
{
    private static GameWindowSettings gameWindowSettings
    {
        get
        {
            GameWindowSettings gws = GameWindowSettings.Default;

            return gws;
        }
    }

    private static NativeWindowSettings nativeWindowSettings
    {
        get
        {
            NativeWindowSettings nws = NativeWindowSettings.Default;

            nws.ClientSize = new Vector2i(1024, 768);
            nws.Title = "Game";
            nws.StartVisible = false;

            return nws;
        }
    }

    private static void Main(string[] args)
    {
        using(Window window = new Window(gameWindowSettings, nativeWindowSettings))
        {
            window.CenterWindow();
            window.IsVisible = true;
            window.Run();
        }
    }
}
