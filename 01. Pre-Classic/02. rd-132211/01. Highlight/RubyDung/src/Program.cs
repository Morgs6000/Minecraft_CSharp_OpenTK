using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using StbImageSharp;

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

            var strem = File.OpenRead("src/textures/opentklogo32x.png");
            var image = ImageResult.FromStream(strem, ColorComponents.RedGreenBlueAlpha);
            var icon = new WindowIcon(new Image(image.Width, image.Height, image.Data));

            nws.Icon = icon;

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
