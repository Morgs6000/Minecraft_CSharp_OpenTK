using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace RubyDung;

public class Window : GameWindow
{
    private Shader shader;
    private Texture texture;
    private Level level;
    private Terrain terrain;
    private ShadedMode shadedMode;
    private Camera camera;
    
    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        Screen.Init(this);

        string shaderPath = "src/shaders/shader.glsl";
        shader = new Shader(shaderPath);

        string texturePath = "src/textures/terrain.png";
        texture = new Texture(texturePath);

        level = new Level(256, 64, 256);
        terrain = new Terrain(level);

        shadedMode = ShadedMode.Shaded;

        camera = new Camera();

        CursorState = CursorState.Grabbed;
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.5f, 0.8f, 1.0f, 1.0f);

        terrain.Load();

        GL.Enable(EnableCap.DepthTest);

        GL.Enable(EnableCap.CullFace);
        GL.CullFace(TriangleFace.Back);
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
        else
        {
            camera.Update();
        }
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        shader.Use();

        Matrix4 model = Matrix4.Identity;
        // model *= Matrix4.CreateFromAxisAngle(new Vector3(0.5f, 1.0f, 0.0f), MathHelper.DegreesToRadians(50.0f) * (float)GLFW.GetTime());
        shader.SetMatrix4("model", model);

        Matrix4 view = Matrix4.Identity;
        view *= camera.LookAt();
        shader.SetMatrix4("view", view);

        Matrix4 projection = Matrix4.Identity;
        projection *= CreatePerspectiveFieldOfView();
        shader.SetMatrix4("projection", projection);

        texture.Bind();

        terrain.Draw(shader, shadedMode);

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
