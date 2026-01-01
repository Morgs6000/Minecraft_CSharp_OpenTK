namespace Minecraft;

public class Block
{
    public static readonly Block[] blocks = new Block[256];

    public static readonly Block empty = null!;
    public static readonly Block rock = new Block(1, 1);
    public static readonly Block grass = new BlockGrass(2);
    public static readonly Block dirt = new Block(3, 2);
    public static readonly Block stoneBrick = new Block(4, 16);
    public static readonly Block wood = new Block(5, 4);
    public static readonly Block bush = new BlockBush(6);
    public static readonly Block unbreakable = new Block(7, 17);

    public int tex;
    public readonly int id;

    protected Block(int id)
    {
        blocks[id] = this;
        this.id = id;
    }

    protected Block(int id, int tex) : this(id)
    {
        this.tex = tex;
    }

    public virtual void Load(Mesh mesh, Level level, int x, int y, int z)
    {
        float c1 = 0.6f;
        float c2 = 1.0f;
        float c3 = 0.8f;

        if (!level.IsSolidBlock(x - 1, y, z))
        {
            mesh.Color(c1, c1, c1);

            LoadFace(mesh, x, y, z, 0);
        }
        if (!level.IsSolidBlock(x + 1, y, z))
        {
            mesh.Color(c1, c1, c1);

            LoadFace(mesh, x, y, z, 1);
        }
        if (!level.IsSolidBlock(x, y - 1, z))
        {
            mesh.Color(c2, c2, c2);

            LoadFace(mesh, x, y, z, 2);
        }
        if (!level.IsSolidBlock(x, y + 1, z))
        {
            mesh.Color(c2, c2, c2);

            LoadFace(mesh, x, y, z, 3);
        }
        if (!level.IsSolidBlock(x, y, z - 1))
        {
            mesh.Color(c3, c3, c3);

            LoadFace(mesh, x, y, z, 4);
        }
        if (!level.IsSolidBlock(x, y, z + 1))
        {
            mesh.Color(c3, c3, c3);

            LoadFace(mesh, x, y, z, 5);
        }
    }

    protected virtual int GetTexture(int face)
    {
        return tex;
    }

    public void LoadFace(Mesh mesh, int x, int y, int z, int face)
    {
        float x0 = (float)x + 0.0f;
        float y0 = (float)y + 0.0f;
        float z0 = (float)z + 0.0f;

        float x1 = (float)x + 1.0f;
        float y1 = (float)y + 1.0f;
        float z1 = (float)z + 1.0f;

        int tex = GetTexture(face);

        float u0 = (float)(tex % 16) / 16.0f;
        float v0 = (float)(tex / 16) / 16.0f;

        float u1 = u0 + (1.0f / 16.0f);
        float v1 = v0 + (1.0f / 16.0f);

        if (face == 0)
        {
            mesh.VertexUV(x0, y0, z0, u0, v1);
            mesh.VertexUV(x0, y0, z1, u1, v1);
            mesh.VertexUV(x0, y1, z1, u1, v0);
            mesh.VertexUV(x0, y1, z0, u0, v0);
        }
        if (face == 1)
        {
            mesh.VertexUV(x1, y0, z1, u0, v1);
            mesh.VertexUV(x1, y0, z0, u1, v1);
            mesh.VertexUV(x1, y1, z0, u1, v0);
            mesh.VertexUV(x1, y1, z1, u0, v0);
        }
        if (face == 2)
        {
            mesh.VertexUV(x0, y0, z0, u0, v1);
            mesh.VertexUV(x1, y0, z0, u1, v1);
            mesh.VertexUV(x1, y0, z1, u1, v0);
            mesh.VertexUV(x0, y0, z1, u0, v0);
        }
        if (face == 3)
        {
            mesh.VertexUV(x0, y1, z1, u0, v1);
            mesh.VertexUV(x1, y1, z1, u1, v1);
            mesh.VertexUV(x1, y1, z0, u1, v0);
            mesh.VertexUV(x0, y1, z0, u0, v0);
        }
        if (face == 4)
        {
            mesh.VertexUV(x1, y0, z0, u0, v1);
            mesh.VertexUV(x0, y0, z0, u1, v1);
            mesh.VertexUV(x0, y1, z0, u1, v0);
            mesh.VertexUV(x1, y1, z0, u0, v0);
        }
        if (face == 5)
        {
            mesh.VertexUV(x0, y0, z1, u0, v1);
            mesh.VertexUV(x1, y0, z1, u1, v1);
            mesh.VertexUV(x1, y1, z1, u1, v0);
            mesh.VertexUV(x0, y1, z1, u0, v0);
        }
    }

    public void LoadFaceNoTexture(Mesh mesh, int x, int y, int z, int face)
    {
        float x0 = (float)x + 0.0f;
        float y0 = (float)y + 0.0f;
        float z0 = (float)z + 0.0f;

        float x1 = (float)x + 1.0f;
        float y1 = (float)y + 1.0f;
        float z1 = (float)z + 1.0f;

        if (face == 0)
        {
            mesh.Vertex(x0, y0, z0);
            mesh.Vertex(x0, y0, z1);
            mesh.Vertex(x0, y1, z1);
            mesh.Vertex(x0, y1, z0);
        }
        if (face == 1)
        {
            mesh.Vertex(x1, y0, z1);
            mesh.Vertex(x1, y0, z0);
            mesh.Vertex(x1, y1, z0);
            mesh.Vertex(x1, y1, z1);
        }
        if (face == 2)
        {
            mesh.Vertex(x0, y0, z0);
            mesh.Vertex(x1, y0, z0);
            mesh.Vertex(x1, y0, z1);
            mesh.Vertex(x0, y0, z1);
        }
        if (face == 3)
        {
            mesh.Vertex(x0, y1, z1);
            mesh.Vertex(x1, y1, z1);
            mesh.Vertex(x1, y1, z0);
            mesh.Vertex(x0, y1, z0);
        }
        if (face == 4)
        {
            mesh.Vertex(x1, y0, z0);
            mesh.Vertex(x0, y0, z0);
            mesh.Vertex(x0, y1, z0);
            mesh.Vertex(x1, y1, z0);
        }
        if (face == 5)
        {
            mesh.Vertex(x0, y0, z1);
            mesh.Vertex(x1, y0, z1);
            mesh.Vertex(x1, y1, z1);
            mesh.Vertex(x0, y1, z1);
        }
    }

    public virtual bool IsSolid()
    {
        return true;
    }
}
