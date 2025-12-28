using OpenTK.Mathematics;
using RubyDung.common;
using RubyDung.level;

namespace RubyDung;

public class Crosshair
{
    // private Shader shader;
    private Mesh mesh;

    private Vector4 color;

    public Crosshair()
    {
        // this.shader = shader;

        mesh = new Mesh();
    }

    public void Load()
    {
        // color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);

        int wc = Screen.widht / 2;
        int hc = Screen.height / 2;

        mesh.Begin();

        /*
        // Linha horizontal
        mesh.Vertex2((float)(wc - 8), (float)(hc + 0));
        mesh.Vertex2((float)(wc + 9), (float)(hc + 0));
        mesh.Vertex2((float)(wc + 9), (float)(hc + 1));
        mesh.Vertex2((float)(wc - 8), (float)(hc + 1));

        // Linha vertical
        mesh.Vertex2((float)(wc + 0), (float)(hc - 8));
        mesh.Vertex2((float)(wc + 1), (float)(hc - 8));
        mesh.Vertex2((float)(wc + 1), (float)(hc + 9));
        mesh.Vertex2((float)(wc + 0), (float)(hc + 9));
        */

        //*
        // Linha horizontal
        mesh.Vertex2(-8.0f,  0.0f);
        mesh.Vertex2( 9.0f,  0.0f);
        mesh.Vertex2( 9.0f,  1.0f);
        mesh.Vertex2(-8.0f,  1.0f);

        // Linha vertical
        mesh.Vertex2( 0.0f, -8.0f);
        mesh.Vertex2( 1.0f, -8.0f);
        mesh.Vertex2( 1.0f,  9.0f);
        mesh.Vertex2( 0.0f,  9.0f);
        //*/

        /*
        // Linha horizontal
        mesh.Vertex2(-5.0f,  0.0f);
        mesh.Vertex2( 4.0f,  0.0f);
        mesh.Vertex2( 4.0f,  1.0f);
        mesh.Vertex2(-5.0f,  1.0f);

        // Linha vertical
        mesh.Vertex2(-1.0f, -4.0f);
        mesh.Vertex2( 0.0f, -4.0f);
        mesh.Vertex2( 0.0f,  5.0f);
        mesh.Vertex2(-1.0f,  5.0f);
        //*/

        mesh.End();
    }

    public void Draw(Shader shader, ShadedMode shadedMode)
    {
        Matrix4 model = Matrix4.Identity;
        shader.SetMatrix4("model", model);

        Matrix4 view = Matrix4.Identity;
        // view *= Matrix4.CreateScale(2.0f);
        view *= Matrix4.CreateTranslation(0.0f, 0.0f, -200.0f);
        shader.SetMatrix4("view", view);

        Matrix4 projection = Matrix4.Identity;
        // projection *= CreateOrthographicOffCenter();
        projection *= CreateOrthographic();
        shader.SetMatrix4("projection", projection);

        shader.SetBool("hasCustomColor", true);
        shader.SetVector4("uniformColor", color);

        mesh.Draw(shader, shadedMode);
    }

    private Matrix4 CreateOrthographicOffCenter()
    {
        float left = 0.0f;
        float right = Screen.widht;
        float bottom = 0.0f;
        float top = Screen.height;
        float depthNear = 100.0f;
        float depthFar = 300.0f;

        return Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, depthNear, depthFar);
    } 
    
    private Matrix4 CreateOrthographic()
    {
        float width = Screen.widht;
        float height = Screen.height;
        float depthNear = 100.0f;
        float depthFar = 300.0f;

        return Matrix4.CreateOrthographic(width, height, depthNear, depthFar);
    }
}
