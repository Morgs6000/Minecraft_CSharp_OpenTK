using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
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

        Matrix4 model = Matrix4.Identity;
        model *= Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.0f, 0.0f), MathHelper.DegreesToRadians(-55.0f));
        shader.SetMatrix4("model", model);

        Matrix4 view = Matrix4.Identity;
        view *= Matrix4.CreateTranslation(new Vector3(0.0f, 0.0f, -3.0f));
        shader.SetMatrix4("view", view);

        Matrix4 projection = Matrix4.Identity;
        projection *= CreatePerspectiveFieldOfView();
        shader.SetMatrix4("projection", projection);

        texture.Bind();
        
        mesh.Draw(shader, shadedMode);

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, Screen.widht, Screen.height);
    }

    private Matrix4 CreatePerspectiveFieldOfView()
    {
        float fovy = MathHelper.DegreesToRadians(70.0f);
        float aspect = (float)Screen.widht / (float)Screen.height;
        float depthNear = 0.05f;
        float depthFar = 1000.0f;

        return Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, depthNear, depthFar);
    }
}
