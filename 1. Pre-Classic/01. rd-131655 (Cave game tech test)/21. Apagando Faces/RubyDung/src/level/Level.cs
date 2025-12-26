namespace RubyDung;

public class Level
{
    public readonly int widht;
    public readonly int height;
    public readonly int depth;

    private byte[,,] blocks;

    public Level(int w, int h, int d)
    {
        widht = w;
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
        if (x >= 0 && x < widht &&
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
