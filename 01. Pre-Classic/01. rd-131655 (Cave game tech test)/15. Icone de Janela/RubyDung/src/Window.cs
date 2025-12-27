using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using RubyDung.Common;
using RubyDung.Level;

namespace RubyDung;

public class Window : GameWindow
{
    private Shader shader;
    private Texture texture;
    private Mesh mesh;

    private ShadedMode shadedMode;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        Screen.Init(this);

        string vertexPath = "src/shaders/vertex.glsl";
        string fragmentPath = "src/shaders/fragment.glsl";
        shader = new Shader(vertexPath, fragmentPath);

        string texturePath = "src/textures/container.jpg";
        texture = new Texture(texturePath);

        mesh = new Mesh();

        shadedMode = ShadedMode.Shaded;
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.5f, 0.8f, 1.0f, 1.0f);

        mesh.Begin();

        // mesh.Color(1.0f, 0.5f, 0.2f);

        mesh.Vertex2UV(-0.5f, -0.5f, 0.0f, 1.0f);
        mesh.Vertex2UV( 0.5f, -0.5f, 1.0f, 1.0f);
        mesh.Vertex2UV( 0.5f,  0.5f, 1.0f, 0.0f);
        mesh.Vertex2UV(-0.5f,  0.5f, 0.0f, 0.0f);
        
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
        if (Input.GetKey(KeyCode.F3))
        {
            if (Input.GetKeyDown(KeyCode.W))
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
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();

        texture.Bind();
        
        mesh.Draw(shader, shadedMode);

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, Screen.widht, Screen.height);
    }
}
