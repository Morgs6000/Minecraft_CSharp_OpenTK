using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RubyDung;

public class Window : GameWindow
{
    private Shader shader;
    private Mesh mesh;
    private ShadedMode shadedMode;
    
    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        Screen.Init(this);

        string shaderPath = "src/shaders/shader.glsl";
        shader = new Shader(shaderPath);

        mesh = new Mesh();

        shadedMode = ShadedMode.Shaded;
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.5f, 0.8f, 1.0f, 1.0f);

        mesh.Begin();

        mesh.Color(1.0f, 0.0f, 0.0f);
        mesh.Vertex2(-0.5f, -0.5f);
        mesh.Color(0.0f, 1.0f, 0.0f);
        mesh.Vertex2( 0.5f, -0.5f);
        mesh.Color(0.0f, 0.0f, 1.0f);
        mesh.Vertex2( 0.5f,  0.5f);
        mesh.Color(1.0f, 1.0f, 0.0f);
        mesh.Vertex2(-0.5f,  0.5f);
        
        mesh.End();
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (shadedMode)
            {
                case ShadedMode.Shaded:
                    shadedMode = ShadedMode.ShadedWireframe;
                    break;
                case ShadedMode.ShadedWireframe:
                    shadedMode = ShadedMode.Wireframe;
                    break;
                case ShadedMode.Wireframe:
                    shadedMode = ShadedMode.Shaded;
                    break;
            }

            Debug.Log($"shadedMode: {shadedMode}");
        }
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();
        
        if (shadedMode == ShadedMode.Shaded || shadedMode == ShadedMode.ShadedWireframe)
        {
            shader.SetBool("hasWireframe", false);
            mesh.Draw(shader);
        }
        if (shadedMode == ShadedMode.Wireframe || shadedMode == ShadedMode.ShadedWireframe)
        {
            shader.SetBool("hasWireframe", true);
            mesh.DrawWireframe();
        }  

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, Screen.widht, Screen.height);
    }
}
