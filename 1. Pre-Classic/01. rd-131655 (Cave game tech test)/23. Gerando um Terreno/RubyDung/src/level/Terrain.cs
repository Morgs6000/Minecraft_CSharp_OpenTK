using RubyDung.common;

namespace RubyDung.level;

public class Terrain
{
    private Level level;
    private Chunk[,,] chunks;

    private int xChunks;
    private int yChunks;
    private int zChunks;

    public Terrain(Level level)
    {
        this.level = level;

        xChunks = level.width  / 16;
        yChunks = level.height / 16;
        zChunks = level.depth  / 16;

        chunks = new Chunk[xChunks, yChunks, zChunks];

        for (int x = 0; x < xChunks; x++)
        {
            for (int y = 0; y < yChunks; y++)
            {
                for (int z = 0; z < zChunks; z++)
                {
                    int x0 = x * 16;
                    int y0 = y * 16;
                    int z0 = z * 16;

                    int x1 = (x + 1) * 16;
                    int y1 = (y + 1) * 16;
                    int z1 = (z + 1) * 16;

                    if (x1 > level.width) {
                        x1 = level.width;
                    }

                    if (y1 > level.height) {
                        y1 = level.height;
                    }

                    if (z1 > level.depth) {
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
}
