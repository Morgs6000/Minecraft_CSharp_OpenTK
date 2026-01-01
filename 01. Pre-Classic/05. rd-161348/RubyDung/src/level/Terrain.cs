namespace RubyDung;

public class Terrain
{
    private static readonly int CHUNK_SIZE = 16;

    private Level level;
    private Chunk[] chunks;

    private int xChunks;
    private int yChunks;
    private int zChunks;

    public Terrain(Level level)
    {
        this.level = level;

        xChunks = level.width  / CHUNK_SIZE;
        yChunks = level.height / CHUNK_SIZE;
        zChunks = level.depth  / CHUNK_SIZE;

        chunks = new Chunk[xChunks * yChunks * zChunks];

        for (int x = 0; x < xChunks; x++)
        {
            for (int y = 0; y < yChunks; y++)
            {
                for (int z = 0; z < zChunks; z++)
                {
                    int x0 = x * CHUNK_SIZE;
                    int y0 = y * CHUNK_SIZE;
                    int z0 = z * CHUNK_SIZE;

                    int x1 = (x + 1) * CHUNK_SIZE;
                    int y1 = (y + 1) * CHUNK_SIZE;
                    int z1 = (z + 1) * CHUNK_SIZE;

                    if(x1 > level.width)
                    {
                        x1 = level.width;
                    }
                    if(y1 > level.height)
                    {
                        y1 = level.height;
                    }
                    if (z1 > level.depth)
                    {
                        z1 = level.depth;
                    }
                    
                    chunks[(x + y * xChunks) * zChunks + z] = new Chunk(
                        level, x0, y0, z0, x1, y1, z1
                    );
                }
            }
        }
    }

    public void Load()
    {
        foreach (Chunk chunk in chunks)
        {
            chunk.Load();
        }        
    }
    
    public void Draw(Shader shader)
    {
        foreach (Chunk chunk in chunks)
        {
            chunk.Draw(shader);
        }        
    }
    
    public void SetChunk(int x0, int y0, int z0, int x1, int y1, int z1)
    {
        x0 /= CHUNK_SIZE;
        x1 /= CHUNK_SIZE;
        y0 /= CHUNK_SIZE;

        y1 /= CHUNK_SIZE;
        z0 /= CHUNK_SIZE;
        z1 /= CHUNK_SIZE;

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

        if (x1 >= this.xChunks)
        {
            x1 = this.xChunks - 1;
        }
        if (y1 >= this.yChunks)
        {
            y1 = this.yChunks - 1;
        }
        if (z1 >= this.zChunks)
        {
            z1 = this.zChunks - 1;
        }

        for (int x = x0; x <= x1; x++)
        {
            for (int y = y0; y <= y1; y++)
            {
                for (int z = z0; z <= z1; z++)
                {
                    chunks[(x + y * xChunks) * zChunks + z].Load();
                }
            }
        }
    }
}
