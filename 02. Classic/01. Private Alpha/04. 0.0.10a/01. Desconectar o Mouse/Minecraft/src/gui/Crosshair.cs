using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace Minecraft;

public class Crosshair
{
    private GameWindow gameWindow;
    private Mesh mesh;

    public Crosshair(GameWindow gameWindow)
    {
        this.gameWindow = gameWindow;

        mesh = new Mesh();
    }

    public void Load()
    {
        int screenWidth = gameWindow.ClientSize.X * 240 / gameWindow.ClientSize.Y;
        int screenHeight = gameWindow.ClientSize.Y * 240 / gameWindow.ClientSize.Y;

        int wc = screenWidth / 2;
        int hc = screenHeight / 2;

        mesh.Begin();

        /*
        mesh.Vertex((float)(wc - 8), (float)(hc - 0), 0.0f);
        mesh.Vertex((float)(wc + 9), (float)(hc - 0), 0.0f);
        mesh.Vertex((float)(wc + 9), (float)(hc + 1), 0.0f);
        mesh.Vertex((float)(wc - 8), (float)(hc + 1), 0.0f);

        mesh.Vertex((float)(wc - 0), (float)(hc - 8), 0.0f);
        mesh.Vertex((float)(wc + 1), (float)(hc - 8), 0.0f);
        mesh.Vertex((float)(wc + 1), (float)(hc + 9), 0.0f);
        mesh.Vertex((float)(wc - 0), (float)(hc + 9), 0.0f);
        //*/
        //*
        mesh.Vertex((float)(wc - 4), (float)(hc - 0), 0.0f);
        mesh.Vertex((float)(wc + 5), (float)(hc - 0), 0.0f);
        mesh.Vertex((float)(wc + 5), (float)(hc + 1), 0.0f);
        mesh.Vertex((float)(wc - 4), (float)(hc + 1), 0.0f);

        mesh.Vertex((float)(wc - 0), (float)(hc - 4), 0.0f);
        mesh.Vertex((float)(wc + 1), (float)(hc - 4), 0.0f);
        mesh.Vertex((float)(wc + 1), (float)(hc + 5), 0.0f);
        mesh.Vertex((float)(wc - 0), (float)(hc + 5), 0.0f);
        //*/

        mesh.End();
    }

    public void Render(Shader shader)
    {
        Matrix4 model = Matrix4.Identity;
        shader.SetMatrix4("model", model);

        Matrix4 view = Matrix4.Identity;
        shader.SetMatrix4("view", view);

        Matrix4 projection = Matrix4.Identity;
        projection *= CreateOrthographicOffCenter();
        shader.SetMatrix4("projection", projection);

        shader.SetBool("hasUniformColor", true);
        shader.SetVector4("uniformColor", 1.0f, 1.0f, 1.0f, 1.0f);

        mesh.Render(shader);
    }
        
    public Matrix4 CreateOrthographicOffCenter()
    {
        int screenWidth = gameWindow.ClientSize.X * 240 / gameWindow.ClientSize.Y;
        int screenHeight = gameWindow.ClientSize.Y * 240 / gameWindow.ClientSize.Y;

        float left = 0.0f;
        float right = (float)screenWidth;
        float bottom = 0.0f;
        float top = (float)screenHeight;
        float depthNear = 0.0f;
        float depthFar = 100.0f;

        return Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, depthNear, depthFar);
    }
}
