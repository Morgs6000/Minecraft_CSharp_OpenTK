using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Minecraft;

public class Window : GameWindow
{
    private Shader shader;
    private Texture texture;

    private Level level;
    private Terrain terrain;

    private Camera camera;
    private Highlight highlight;

    private Crosshair crosshair;
    private DrawGUI drawGUI;
    private Font font;

    private bool fullscreen = false;
    private bool pause = false;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        WindowState = fullscreen ? WindowState.Fullscreen : WindowState.Normal;

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
        font = new Font(this);
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.5f, 0.8f, 1.0f, 1.0f);

        terrain.Load();

        crosshair.Load();
        drawGUI.Load();
        font.Load();

        // GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);

        GL.Enable(EnableCap.DepthTest);

        GL.Enable(EnableCap.CullFace);
        GL.CullFace(TriangleFace.Back);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        // if (KeyboardState.IsKeyDown(Keys.Escape))
        // {
        //     Close();
        // }
        if (KeyboardState.IsKeyPressed(Keys.Enter))
        {
            level.Save();
        }

        if (KeyboardState.IsKeyPressed(Keys.Escape))
        {
            pause = !pause;
        }
        if (pause)
        {
            CursorState = CursorState.Normal;
        }
        else
        {
            CursorState = CursorState.Grabbed;
            
            highlight.Update(this, drawGUI);
            drawGUI.Update();
            font.Update();
        }

        camera.Update(this, pause);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        // if (!pause)
        // {
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

            terrain.Render(shader);
            highlight.Render(shader);

            crosshair.Render(shader);
            drawGUI.Render(shader);
            font.Render(shader);

            SwapBuffers();
        // }        
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
