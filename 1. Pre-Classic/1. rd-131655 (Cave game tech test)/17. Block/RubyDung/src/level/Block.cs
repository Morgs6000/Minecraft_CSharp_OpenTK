namespace RubyDung;

public class Block
{
    public static Block block = new Block();

    public void Render(Mesh mesh)
    {
        float x0 = -0.5f;
        float y0 = -0.5f;
        float z0 = -0.5f;

        float x1 =  0.5f;
        float y1 =  0.5f;
        float z1 =  0.5f;

        float u0 = 0.0f;
        float v0 = 0.0f;

        float u1 = 1.0f;
        float v1 = 1.0f;

        mesh.VertexUV(x0, y0, z0, u0, v1);
        mesh.VertexUV(x0, y0, z1, u1, v1);
        mesh.VertexUV(x0, y1, z1, u1, v0);
        mesh.VertexUV(x0, y1, z0, u0, v0);

        mesh.VertexUV(x1, y0, z1, u0, v1);
        mesh.VertexUV(x1, y0, z0, u1, v1);
        mesh.VertexUV(x1, y1, z0, u1, v0);
        mesh.VertexUV(x1, y1, z1, u0, v0);

        mesh.VertexUV(x0, y0, z0, u0, v1);
        mesh.VertexUV(x1, y0, z0, u1, v1);
        mesh.VertexUV(x1, y0, z1, u1, v0);
        mesh.VertexUV(x0, y0, z1, u0, v0);

        mesh.VertexUV(x0, y1, z1, u0, v1);
        mesh.VertexUV(x1, y1, z1, u1, v1);
        mesh.VertexUV(x1, y1, z0, u1, v0);
        mesh.VertexUV(x0, y1, z0, u0, v0);

        mesh.VertexUV(x1, y0, z0, u0, v1);
        mesh.VertexUV(x0, y0, z0, u1, v1);
        mesh.VertexUV(x0, y1, z0, u1, v0);
        mesh.VertexUV(x1, y1, z0, u0, v0);

        mesh.VertexUV(x0, y0, z1, u0, v1);
        mesh.VertexUV(x1, y0, z1, u1, v1);
        mesh.VertexUV(x1, y1, z1, u1, v0);
        mesh.VertexUV(x0, y1, z1, u0, v0);
    }
}
