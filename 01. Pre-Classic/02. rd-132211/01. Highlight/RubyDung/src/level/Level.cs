namespace RubyDung;

public class Level
{
    public readonly int width;
    public readonly int height;
    public readonly int depth;

    private byte[] blocks;

    public Level(int w, int h, int d)
    {
        width = w;
        height = h;
        depth = d;

        blocks = new byte[w * h * d];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                for (int z = 0; z < d; z++)
                {
                    int i = (x + y * width) * depth + z;
                    blocks[i] = (byte)(y <= h * 2 / 3 ? 1 : 0);
                }
            }
        }
    }

    public bool IsBlock(int x, int y, int z)
    {
        if (x >= 0 && x < width &&
           y >= 0 && y < height &&
           z >= 0 && z < depth)
        {
            return blocks[(x + y * width) * depth + z] == 1;
        }

        return false;
    }

    public bool IsSolidBlock(int x, int y, int z)
    {
        return IsBlock(x, y, z);
    }

    public List<AABB> GetCubes(AABB other)
    {
        List<AABB> cubes = new List<AABB>();

        int x0 = (int)other.x0;
        int y0 = (int)other.y0;
        int z0 = (int)other.z0;

        int x1 = (int)(other.x1 + 1.0F);
        int y1 = (int)(other.y1 + 1.0F);
        int z1 = (int)(other.z1 + 1.0F);

        if (x0 < 0)
        {
            x0 = 0;
        }
        if (y0 < 0)
        {
            y0 = 0;
        }
        if (z0 < 0)
        {
            z0 = 0;
        }

        if (x1 > this.width)
        {
            x1 = this.width;
        }
        if (y1 > this.height)
        {
            y1 = this.height;
        }
        if (z1 > this.depth)
        {
            z1 = this.depth;
        }

        for (int x = x0; x < x1; x++)
        {
            for (int y = y0; y < y1; y++)
            {
                for (int z = z0; z < z1; z++)
                {
                    if (this.IsSolidBlock(x, y, z))
                    {
                        cubes.Add(new AABB(
                            (float)x, (float)y, (float)z,
                            (float)(x + 1), (float)(y + 1), (float)(z + 1))
                        );
                    }
                }
            }
        }

        return cubes;
    }
}
