namespace Minecraft;

public class BlockBush : Block
{
    public BlockBush(int id) : base(id)
    {
        tex = 15;
    }

    public override void Load(Mesh mesh, Level level, int x, int y, int z)
    {
        int tex = GetTexture(15);

        float u0 = (float)(tex % 16) / 16.0f;
        float v0 = (float)(tex / 16) / 16.0f;

        float u1 = u0 + (1.0f / 16.0f);
        float v1 = v0 + (1.0f / 16.0f);

        mesh.Color(1.0f, 1.0f, 1.0f);

        int rots = 2;

        for (int r = 0; r < rots; r++)
        {
            float xa = (float)(Math.Sin(r * Math.PI / rots + (Math.PI / 4.0f)) * 0.5f);
            float za = (float)(Math.Cos(r * Math.PI / rots + (Math.PI / 4.0f)) * 0.5f);

            float x0 = (float)x + 0.5f - xa;
            float y0 = (float)y + 0.0f;
            float z0 = (float)z + 0.5f - za;

            float x1 = (float)x + 0.5f + xa;
            float y1 = (float)y + 1.0f;
            float z1 = (float)z + 0.5f + za;

            mesh.VertexUV(x0, y1, z0, u1, v0);
            mesh.VertexUV(x1, y1, z1, u0, v0);
            mesh.VertexUV(x1, y0, z1, u0, v1);
            mesh.VertexUV(x0, y0, z0, u1, v1);

            mesh.VertexUV(x1, y1, z1, u0, v0);
            mesh.VertexUV(x0, y1, z0, u1, v0);
            mesh.VertexUV(x0, y0, z0, u1, v1);
            mesh.VertexUV(x1, y0, z1, u0, v1);
        }
    }

    public override bool IsSolid()
    {
        return false;
    }
}
