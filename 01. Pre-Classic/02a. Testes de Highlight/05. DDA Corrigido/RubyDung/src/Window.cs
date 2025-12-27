using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using RubyDung.common;
using RubyDung.level;

namespace RubyDung;

public class Window : GameWindow
{
    private Shader shader;
    private Texture texture;

    private Level level;
    private Terrain terrain;

    private Camera camera;
    private Highlight highlight;
    private Crosshair crosshair;

    private ShadedMode shadedMode;
    private bool hasPause = false;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        Screen.Init(this);

        string vertexPath = "src/shaders/default/vertex.glsl";
        string fragmentPath = "src/shaders/default/fragment.glsl";
        shader = new Shader(vertexPath, fragmentPath);

        string texturePath = "src/textures/terrain.png";
        texture = new Texture(texturePath);

        // level = new Level(256, 64, 256);
        level = new Level(16, 16, 16);
        terrain = new Terrain(level);

        camera = new Camera(level);
        highlight = new Highlight(level, camera);
        crosshair = new Crosshair(shader);

        shadedMode = ShadedMode.Shaded;
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.5f, 0.8f, 1.0f, 1.0f);

        terrain.Load();

        crosshair.Load();

        GL.Enable(EnableCap.DepthTest);

        GL.Enable(EnableCap.CullFace);
        GL.CullFace(TriangleFace.Back);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        Time.Update();
        Input.Update(this);
        GameCursor.Update(this);

        if (Input.GetKey(KeyCode.Escape))
        {
            Close();
            // hasPause = true;
            // hasPause = !hasPause;
        }

        // if (IsFocused)
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         hasPause = false;
        //     }
        // }
        
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
            camera.Update(hasPause);
            highlight.Update();
        }
        

        GameCursor.lockState = hasPause ? CursorLockMode.None : CursorLockMode.Locked;
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        shader.Use();

        Matrix4 model = Matrix4.Identity;
        shader.SetMatrix4("model", model);

        Matrix4 view = Matrix4.Identity;
        view *= camera.LookAt();
        shader.SetMatrix4("view", view);

        Matrix4 projection = Matrix4.Identity;
        projection *= camera.CreatePerspectiveFieldOfView();
        shader.SetMatrix4("projection", projection);

        texture.Bind();

        terrain.Draw(shader, shadedMode);

        highlight.Draw(shadedMode);
        crosshair.Draw(shadedMode);

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, Screen.widht, Screen.height);
    }
}
