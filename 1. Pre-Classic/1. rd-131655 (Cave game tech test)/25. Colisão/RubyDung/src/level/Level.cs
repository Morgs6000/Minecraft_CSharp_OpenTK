using RubyDung.phys;

namespace RubyDung;

public class Level
{
    public readonly int width;
    public readonly int height;
    public readonly int depth;

    private byte[,,] blocks;

    public Level(int w, int h, int d)
    {
        width = w;
        height = h;
        depth = d;

        blocks = new byte[w, h, d];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                for (int z = 0; z < d; z++)
                {
                    blocks[x, y, z] = (byte)(y <= h * 2 / 3 ? 1 : 0);
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
            return blocks[x, y, z] == 1;
        }
        
        return false;        
    }

    public bool IsSolidBlock(int x, int y, int z)
    {
        return IsBlock(x, y, z);
    }
    
    public List<AABB> GetCubes(AABB cameraBox)
    {
        List<AABB> blockBoxs = new List<AABB>();

        int x0 = (int)cameraBox.x0;
        int y0 = (int)cameraBox.y0;
        int z0 = (int)cameraBox.z0;

        int x1 = (int)(cameraBox.x1 + 1.0f);
        int y1 = (int)(cameraBox.y1 + 1.0f);
        int z1 = (int)(cameraBox.z1 + 1.0f);

        if(x0 < 0)
        {
            x0 = 0;
        }
        if(y0 < 0)
        {
            y0 = 0;
        }
        if (x0 < 0)
        {
            z0 = 0;
        }
        
        if(x1 > width)
        {
            x1 = width;
        }
        if(y1 > height)
        {
            y1 = height;
        }
        if (z1 > depth)
        {
            z1 = depth;
        }

        for (int x = x0; x < x1; x++)
        {
            for (int y = y0; y < y1; y++)
            {
                for (int z = z0; z < z1; z++)
                {
                    if (IsSolidBlock(x, y, z))
                    {
                        blockBoxs.Add(new AABB(
                            (float)x, (float)y, (float)z,
                            (float)(x + 1), (float)(y + 1), (float)(z + 1)
                        ));
                    }
                }
            }
        }

        return blockBoxs;
    }
}
