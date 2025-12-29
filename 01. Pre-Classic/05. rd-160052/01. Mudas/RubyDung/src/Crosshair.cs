using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace RubyDung;

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
        int wc = gameWindow.ClientSize.X / 2;
        int hc = gameWindow.ClientSize.Y / 2;

        mesh.Begin();

        mesh.Vertex((float)(wc - 8), (float)(hc - 0), 0.0f);
        mesh.Vertex((float)(wc + 9), (float)(hc - 0), 0.0f);
        mesh.Vertex((float)(wc + 9), (float)(hc + 1), 0.0f);
        mesh.Vertex((float)(wc - 8), (float)(hc + 1), 0.0f);

        mesh.Vertex((float)(wc - 0), (float)(hc - 8), 0.0f);
        mesh.Vertex((float)(wc + 1), (float)(hc - 8), 0.0f);
        mesh.Vertex((float)(wc + 1), (float)(hc + 9), 0.0f);
        mesh.Vertex((float)(wc - 0), (float)(hc + 9), 0.0f);

        mesh.End();
    }

    public void Draw(Shader shader)
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

        mesh.Draw(shader);
    }
        
    public Matrix4 CreateOrthographicOffCenter()
    {
        float left = 0.0f;
        float right = (float)gameWindow.ClientSize.X;
        float bottom = 0.0f;
        float top = (float)gameWindow.ClientSize.Y;
        float depthNear = 0.0f;
        float depthFar = 100.0f;

        return Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, depthNear, depthFar);
    }
}
