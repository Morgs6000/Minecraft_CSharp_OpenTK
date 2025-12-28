namespace RubyDung.level;

public class Block
{
    public static Block grass = new Block(0);
    public static Block rock = new Block(1);

    private int tex = 0;

    private Block(int tex)
    {
        this.tex = tex;
    }

    public void Load(Mesh mesh, Level level, int x, int y, int z)
    {
        float x0 = (float)x + 0.0f;
        float y0 = (float)y + 0.0f;
        float z0 = (float)z + 0.0f;

        float x1 = (float)x + 1.0f;
        float y1 = (float)y + 1.0f;
        float z1 = (float)z + 1.0f;

        float c1 = 0.6f;
        float c2 = 1.0f;
        float c3 = 0.8f;

        float u0 = (float)(tex % 16) / 16.0f;
        float v0 = (float)(tex / 16) / 16.0f;

        float u1 = u0 + (1.0f / 16.0f);
        float v1 = v0 + (1.0f / 16.0f);

        if (!level.IsSolidBlock(x - 1, y, z))
        {
            float br = c1;
            mesh.Color(br, br, br);

            mesh.Vertex3_UV(x0, y0, z0, u0, v1);
            mesh.Vertex3_UV(x0, y0, z1, u1, v1);
            mesh.Vertex3_UV(x0, y1, z1, u1, v0);
            mesh.Vertex3_UV(x0, y1, z0, u0, v0);
        }
        if (!level.IsSolidBlock(x + 1, y, z))
        {
            float br = c1;
            mesh.Color(br, br, br);

            mesh.Vertex3_UV(x1, y0, z1, u0, v1);
            mesh.Vertex3_UV(x1, y0, z0, u1, v1);
            mesh.Vertex3_UV(x1, y1, z0, u1, v0);
            mesh.Vertex3_UV(x1, y1, z1, u0, v0);
        }
        if (!level.IsSolidBlock(x, y - 1, z))
        {
            float br = c2;
            mesh.Color(br, br, br);

            mesh.Vertex3_UV(x0, y0, z0, u0, v1);
            mesh.Vertex3_UV(x1, y0, z0, u1, v1);
            mesh.Vertex3_UV(x1, y0, z1, u1, v0);
            mesh.Vertex3_UV(x0, y0, z1, u0, v0);
        }
        if (!level.IsSolidBlock(x, y + 1, z))
        {
            float br = c2;
            mesh.Color(br, br, br);

            mesh.Vertex3_UV(x0, y1, z1, u0, v1);
            mesh.Vertex3_UV(x1, y1, z1, u1, v1);
            mesh.Vertex3_UV(x1, y1, z0, u1, v0);
            mesh.Vertex3_UV(x0, y1, z0, u0, v0);
        }
        if (!level.IsSolidBlock(x, y, z - 1))
        {
            float br = c3;
            mesh.Color(br, br, br);

            mesh.Vertex3_UV(x1, y0, z0, u0, v1);
            mesh.Vertex3_UV(x0, y0, z0, u1, v1);
            mesh.Vertex3_UV(x0, y1, z0, u1, v0);
            mesh.Vertex3_UV(x1, y1, z0, u0, v0);
        }
        if (!level.IsSolidBlock(x, y, z + 1))
        {
            float br = c3;
            mesh.Color(br, br, br);

            mesh.Vertex3_UV(x0, y0, z1, u0, v1);
            mesh.Vertex3_UV(x1, y0, z1, u1, v1);
            mesh.Vertex3_UV(x1, y1, z1, u1, v0);
            mesh.Vertex3_UV(x0, y1, z1, u0, v0);
        }        
    }

    public void LoadFace(Mesh mesh, int x, int y, int z, int face)
    {
        float x0 = (float)x + 0.0f;
        float y0 = (float)y + 0.0f;
        float z0 = (float)z + 0.0f;

        float x1 = (float)x + 1.0f;
        float y1 = (float)y + 1.0f;
        float z1 = (float)z + 1.0f;

        if (face == 0)
        {
            mesh.Vertex3(x0, y0, z0);
            mesh.Vertex3(x0, y0, z1);
            mesh.Vertex3(x0, y1, z1);
            mesh.Vertex3(x0, y1, z0);
        }
        if (face == 1)
        {
            mesh.Vertex3(x1, y0, z1);
            mesh.Vertex3(x1, y0, z0);
            mesh.Vertex3(x1, y1, z0);
            mesh.Vertex3(x1, y1, z1);
        }
        if (face == 2)
        {
            mesh.Vertex3(x0, y0, z0);
            mesh.Vertex3(x1, y0, z0);
            mesh.Vertex3(x1, y0, z1);
            mesh.Vertex3(x0, y0, z1);
        }
        if (face == 3)
        {
            mesh.Vertex3(x0, y1, z1);
            mesh.Vertex3(x1, y1, z1);
            mesh.Vertex3(x1, y1, z0);
            mesh.Vertex3(x0, y1, z0);
        }
        if (face == 4)
        {
            mesh.Vertex3(x1, y0, z0);
            mesh.Vertex3(x0, y0, z0);
            mesh.Vertex3(x0, y1, z0);
            mesh.Vertex3(x1, y1, z0);
        }
        if (face == 5)
        {
            mesh.Vertex3(x0, y0, z1);
            mesh.Vertex3(x1, y0, z1);
            mesh.Vertex3(x1, y1, z1);
            mesh.Vertex3(x0, y1, z1);
        }
    }
}
