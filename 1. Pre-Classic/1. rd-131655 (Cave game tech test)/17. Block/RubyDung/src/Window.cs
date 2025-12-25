using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

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

        string shaderPath = "src/shaders/shader.glsl";
        shader = new Shader(shaderPath);

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
        Block.block.Render(mesh);
        mesh.End();

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

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        shader.Use();

        Matrix4 model = Matrix4.Identity;
        model *= Matrix4.CreateFromAxisAngle(new Vector3(0.5f, 1.0f, 0.0f), MathHelper.DegreesToRadians(50.0f) * (float)GLFW.GetTime());
        shader.SetMatrix4("model", model);

        Matrix4 view = Matrix4.Identity;
        view *= Matrix4.CreateTranslation(new Vector3(0.0f, 0.0f, -3.0f));
        shader.SetMatrix4("view", view);

        Matrix4 projection = Matrix4.Identity;
        projection *= Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)Screen.widht / (float)Screen.height, 0.1f, 100.0f);
        shader.SetMatrix4("projection", projection);

        texture.Bind();
        
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
