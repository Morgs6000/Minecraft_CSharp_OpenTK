using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RubyDung;

public class DrawGUI
{
    private GameWindow gameWindow;
    private Shader shader;
    private Level level;
    private Mesh mesh;

    public int paintTexture = 1;

    public DrawGUI(GameWindow gameWindow, Level level)
    {
        this.gameWindow = gameWindow;
        this.level = level;

        string vertexPath = "res/shaders/default/vertex.glsl";
        string fragmentPath = "res/shaders/default/fragment.glsl";
        shader = new Shader(vertexPath, fragmentPath);

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

    public void Draw()
    {
        shader.Use();

        Matrix4 model = Matrix4.Identity;
        shader.SetMatrix4("model", model);

        /*
        Matrix4 view = Matrix4.Identity;
        view *= Matrix4.CreateScale(48.0f, 48.0f, 48.0f);
        shader.SetMatrix4("view", view);
        //*/
        //*
        Matrix4 view = Matrix4.Identity;

        view *= Matrix4.CreateTranslation(1.5f, -0.5f, -0.5f);
        view *= Matrix4.CreateFromAxisAngle(new Vector3(0.0f, 1.0f, 0.0f), MathHelper.DegreesToRadians(-45.0f));
        view *= Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.0f, 0.0f), MathHelper.DegreesToRadians(30.0f));
        view *= Matrix4.CreateScale(48.0f, 48.0f, 48.0f);

        view *= Matrix4.CreateTranslation((float)(gameWindow.ClientSize.X - 48), (float)(gameWindow.ClientSize.Y - 48), 0.0f);

        // view *= Matrix4.CreateTranslation(0.0f, 0.0f, -100.0f);

        shader.SetMatrix4("view", view);
        //*/

        Matrix4 projection = Matrix4.Identity;
        projection *= CreateOrthographicOffCenter();
        shader.SetMatrix4("projection", projection);

        shader.SetBool("hasUniformColor", false);

        mesh.Draw(shader);
    }

    public Matrix4 CreateOrthographicOffCenter()
    {
        float left = 0.0f;
        float right = (float)gameWindow.ClientSize.X;
        float bottom = 0.0f;
        float top = (float)gameWindow.ClientSize.Y;
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
