namespace RubyDung.level;

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
                    blocks[x, y, z] = 1;
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
}
