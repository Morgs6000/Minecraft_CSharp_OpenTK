using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RubyDung;

public class Window : GameWindow
{
    private bool FULLSCREEN_MODE = false;

    private Shader shader;
    private Texture texture;

    private Level level;
    private Terrain terrain;

    private Camera camera;
    private Highlight highlight;

    private Crosshair crosshair;
    private DrawGUI drawGUI;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        WindowState = FULLSCREEN_MODE ? WindowState.Fullscreen : WindowState.Normal;

        string vertexPath = "res/shaders/default/vertex.glsl";
        string fragmentPath = "res/shaders/default/fragment.glsl";
        shader = new Shader(vertexPath, fragmentPath);

        string texturePath = "res/textures/terrain.png";
        texture = new Texture(texturePath);

        level = new Level(256, 64, 256);
        terrain = new Terrain(level);

        camera = new Camera(level);
        highlight = new Highlight(level, terrain, camera);

        crosshair = new Crosshair(this);
        drawGUI = new DrawGUI(this, level);
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.5f, 0.8f, 1.0f, 1.0f);

        terrain.Load();

        crosshair.Load();
        drawGUI.Load();

        // GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);

        GL.Enable(EnableCap.DepthTest);

        GL.Enable(EnableCap.CullFace);
        GL.CullFace(TriangleFace.Back);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
        if (KeyboardState.IsKeyPressed(Keys.Enter))
        {
            level.Save();
        }

        camera.Update(this);
        highlight.Update(this, drawGUI);
        drawGUI.Update();
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
        projection *= camera.CreatePerspectiveFieldOfView(this);
        shader.SetMatrix4("projection", projection);

        shader.SetBool("hasUniformColor", false);

        texture.Bind();

        terrain.Draw(shader);
        highlight.Draw(shader);

        crosshair.Draw(shader);
        drawGUI.Draw();

        SwapBuffers();
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        level.Save();
    }
}
