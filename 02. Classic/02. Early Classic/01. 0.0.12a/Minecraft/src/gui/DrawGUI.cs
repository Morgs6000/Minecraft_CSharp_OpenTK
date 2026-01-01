using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Minecraft;

public class DrawGUI
{
    private GameWindow gameWindow;
    private Level level;
    private Mesh mesh;

    public int paintTexture = 1;

    public DrawGUI(GameWindow gameWindow, Level level)
    {
        this.gameWindow = gameWindow;
        this.level = level;

        mesh = new Mesh();
    }

    public void Load()
    {
        BlockGUI();
    }

    public void Update()
    {
        KeyboardState keyboardState = gameWindow.KeyboardState;

        if (keyboardState.IsKeyPressed(Keys.D1))
        {
            paintTexture = 1;
            BlockGUI();
        }
        if (keyboardState.IsKeyPressed(Keys.D2))
        {
            paintTexture = 3;
            BlockGUI();
        }
        if (keyboardState.IsKeyPressed(Keys.D3))
        {
            paintTexture = 4;
            BlockGUI();
        }
        if (keyboardState.IsKeyPressed(Keys.D4))
        {
            paintTexture = 5;
            BlockGUI();
        }
        if (keyboardState.IsKeyPressed(Keys.D6))
        {
            paintTexture = 6;
            BlockGUI();
        }
    }

    public void Render(Shader shader)
    {
        GL.Disable(EnableCap.DepthTest);

        shader.Use();

        Matrix4 model = Matrix4.Identity;
        shader.SetMatrix4("model", model);

        int screenWidth = gameWindow.ClientSize.X * 240 / gameWindow.ClientSize.Y;
        int screenHeight = gameWindow.ClientSize.Y * 240 / gameWindow.ClientSize.Y;

        /*
        Matrix4 view = Matrix4.Identity;
        view *= Matrix4.CreateScale(48.0f, 48.0f, 48.0f);
        shader.SetMatrix4("view", view);
        //*/
        //*
        Matrix4 view = Matrix4.Identity;

        // view *= Matrix4.CreateScale(-1.0f, -1.0f, -1.0f);
        view *= Matrix4.CreateTranslation(1.5f, -0.5f, -0.5f);
        view *= Matrix4.CreateFromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), MathHelper.DegreesToRadians(-45.0f));
        view *= Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.0f, 0.0f), MathHelper.DegreesToRadians(30.0f));
        view *= Matrix4.CreateScale(16.0f, 16.0f, 16.0f);

        view *= Matrix4.CreateTranslation((float)(screenWidth - 16), (float)(screenHeight - 16), 0.0f);

        // view *= Matrix4.CreateTranslation(0.0f, 0.0f, -100.0f);

        shader.SetMatrix4("view", view);
        //*/

        Matrix4 projection = Matrix4.Identity;
        projection *= CreateOrthographicOffCenter();
        shader.SetMatrix4("projection", projection);

        shader.SetBool("hasUniformColor", false);

        mesh.Render(shader);

        GL.Enable(EnableCap.DepthTest);
    }

    public Matrix4 CreateOrthographicOffCenter()
    {   
        int screenWidth = gameWindow.ClientSize.X * 240 / gameWindow.ClientSize.Y;
        int screenHeight = gameWindow.ClientSize.Y * 240 / gameWindow.ClientSize.Y;

        float left = 0.0f;
        float right = (float)screenWidth;
        float bottom = 0.0f;
        float top = (float)screenHeight;
        float depthNear = -100.0f;
        float depthFar = 100.0f;

        return Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, depthNear, depthFar);
    }
    
    private void BlockGUI()
    {
        mesh.Begin();

        Block.blocks[paintTexture].Load(mesh, level, -2, 0, 0);

        /*
        float c1 = 0.6f;
        float c2 = 1.0f;
        float c3 = 0.8f;

        // mesh.Color(c1, c1, c1);
        // Block.blocks[1].LoadFace(mesh, -2, 0, 0, 0);
        mesh.Color(c1, c1, c1);
        Block.blocks[1].LoadFace(mesh, -2, 0, 0, 1);
        // mesh.Color(c2, c2, c2);
        // Block.blocks[1].LoadFace(mesh, -2, 0, 0, 2);
        mesh.Color(c2, c2, c2);
        Block.blocks[1].LoadFace(mesh, -2, 0, 0, 3);
        // mesh.Color(c3, c3, c3);
        // Block.blocks[1].LoadFace(mesh, -2, 0, 0, 4);
        mesh.Color(c3, c3, c3);
        Block.blocks[1].LoadFace(mesh, -2, 0, 0, 5);
        //*/

        /*
        for (int face = 0; face < 6; face++)
        {
            Block.blocks[1].LoadFace(mesh, 0, 0, 0, face);
        }
        //*/

        mesh.End();
    }
}
