using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using RubyDung.Common;

namespace RubyDung;

public class Window : GameWindow
{
    private Shader shader;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        Screen.Init(this);

        string vertexPath = "src/shaders/vertex.glsl";
        string fragmentPath = "src/shaders/fragment.glsl";
        shader = new Shader(vertexPath, fragmentPath);
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.5f, 0.8f, 1.0f, 1.0f);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        Time.Update();
        Input.Update(this);

        if (Input.GetKey(KeyCode.Escape))
        {
            Close();
        }
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, Screen.widht, Screen.height);
    }
}
