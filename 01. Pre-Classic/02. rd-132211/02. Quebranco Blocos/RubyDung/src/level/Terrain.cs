using RubyDung.common;

namespace RubyDung.level;

public class Terrain
{
    private Level level;
    private Chunk[,,] chunks;
    private readonly int CHUNK_SIZE = 16;

    private int xChunks;
    private int yChunks;
    private int zChunks;

    public Terrain(Level level)
    {
        this.level = level;

        xChunks = level.width  / CHUNK_SIZE;
        yChunks = level.height / CHUNK_SIZE;
        zChunks = level.depth  / CHUNK_SIZE;

        chunks = new Chunk[xChunks, yChunks, zChunks];

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

                    if (x1 > level.width)
                    {
                        x1 = level.width;
                    }
                    if (y1 > level.height)
                    {
                        y1 = level.height;
                    }
                    if (z1 > level.depth)
                    {
                        z1 = level.depth;
                    }

                    chunks[x, y, z] = new Chunk(level, x0, y0, z0, x1, y1, z1);
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

    public void Draw(Shader shader, ShadedMode shadedMode)
    {
        foreach (Chunk chunk in chunks)
        {
            chunk.Draw(shader, shadedMode);
        }
    }

    /*
    public void ChunkReload(int x, int y, int z)
    {
        if (x >= 0 && x < xChunks &&
            y >= 0 && y < yChunks &&
            z >= 0 && z < zChunks)
        {
            chunks[x, y, z].Load();
        }
    }
    */
    
    public void ChunkReload(int x, int y, int z)
    {
        // ChunkReload(x, y, z);

        if (x >= 0 && x < xChunks &&
            y >= 0 && y < yChunks &&
            z >= 0 && z < zChunks)
        {
            chunks[x, y, z].Load();

            if (x > 0)
            {
                // ChunkReload(x - 1, y, z);
                chunks[x - 1, y, z].Load();
            }
            if (y > 0)
            {
                // ChunkReload(x, y - 1, z);
                chunks[x, y - 1, z].Load();
            }
            if (z > 0)
            {
                // ChunkReload(x, y, z - 1);
                chunks[x, y, z - 1].Load();
            }

            if (x < xChunks - 1)
            {
                // ChunkReload(x + 1, y, z);
                chunks[x + 1, y, z].Load();
            }
            if (y < yChunks - 1)
            {
                // ChunkReload(x, y + 1, z);
                chunks[x, y + 1, z].Load();
            }
            if (z < zChunks - 1)
            {
                // ChunkReload(x, y, z + 1);
                chunks[x, y, z + 1].Load();
            }
        }
    }
}
