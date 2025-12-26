namespace RubyDung.level;

public class Block
{
    public static Block block = new Block();

    public void Load(Mesh mesh, Level level, int x, int y, int z)
    {
        float x0 = (float)x + 0.0f;
        float y0 = (float)y + 0.0f;
        float z0 = (float)z + 0.0f;

        float x1 = (float)x + 1.0f;
        float y1 = (float)y + 1.0f;
        float z1 = (float)z + 1.0f;

        float u0 = 0.0f;
        float v0 = 0.0f;

        float u1 = 1.0f;
        float v1 = 1.0f;

        if (!level.IsSolidBlock(x - 1, y, z))
        {
            mesh.Vertex3_UV(x0, y0, z0, u0, v1);
            mesh.Vertex3_UV(x0, y0, z1, u1, v1);
            mesh.Vertex3_UV(x0, y1, z1, u1, v0);
            mesh.Vertex3_UV(x0, y1, z0, u0, v0);
        }

        if (!level.IsSolidBlock(x + 1, y, z))
        {
            mesh.Vertex3_UV(x1, y0, z1, u0, v1);
            mesh.Vertex3_UV(x1, y0, z0, u1, v1);
            mesh.Vertex3_UV(x1, y1, z0, u1, v0);
            mesh.Vertex3_UV(x1, y1, z1, u0, v0);
        }

        if (!level.IsSolidBlock(x, y - 1, z))
        {
            mesh.Vertex3_UV(x0, y0, z0, u0, v1);
            mesh.Vertex3_UV(x1, y0, z0, u1, v1);
            mesh.Vertex3_UV(x1, y0, z1, u1, v0);
            mesh.Vertex3_UV(x0, y0, z1, u0, v0);
        }

        if (!level.IsSolidBlock(x, y + 1, z))
        {
            mesh.Vertex3_UV(x0, y1, z1, u0, v1);
            mesh.Vertex3_UV(x1, y1, z1, u1, v1);
            mesh.Vertex3_UV(x1, y1, z0, u1, v0);
            mesh.Vertex3_UV(x0, y1, z0, u0, v0);
        }

        if (!level.IsSolidBlock(x, y, z - 1))
        {
            mesh.Vertex3_UV(x1, y0, z0, u0, v1);
            mesh.Vertex3_UV(x0, y0, z0, u1, v1);
            mesh.Vertex3_UV(x0, y1, z0, u1, v0);
            mesh.Vertex3_UV(x1, y1, z0, u0, v0);
        }

        if (!level.IsSolidBlock(x, y, z + 1))
        {
            mesh.Vertex3_UV(x0, y0, z1, u0, v1);
            mesh.Vertex3_UV(x1, y0, z1, u1, v1);
            mesh.Vertex3_UV(x1, y1, z1, u1, v0);
            mesh.Vertex3_UV(x0, y1, z1, u0, v0);
        }
        
    }
}
