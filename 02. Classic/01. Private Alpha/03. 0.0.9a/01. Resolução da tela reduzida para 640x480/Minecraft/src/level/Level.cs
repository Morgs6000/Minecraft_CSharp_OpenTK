using System.IO.Compression;
using System.Text.Json;

namespace Minecraft;

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

        bool mapLoaded = Load();

        if (!mapLoaded)
        {
            GenerateMap();
        }        
    }
    
    private void GenerateMap()
    {
        int w = width;
        int h = height;
        int d = depth;

        int[] heightmap1 = new PerlinNoise(0).Read(w, d);
        int[] heightmap2 = new PerlinNoise(0).Read(w, d);
        int[] cf = new PerlinNoise(1).Read(w, d);
        int[] rockMap = new PerlinNoise(1).Read(w, d);

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                for (int z = 0; z < d; z++)
                {
                    int dh1 = heightmap1[x + z * this.width];
                    int dh2 = heightmap2[x + z * this.width];
                    int cfh = cf[x + z * this.width];
                    
                    if (cfh < 128)
                    {
                        dh2 = dh1;
                    }

                    int dh = dh1;
                    if (dh2 > dh1)
                    {
                        dh = dh2;
                    }

                    dh = dh / 8 + h / 3;
                    int rh = rockMap[x + z * this.width] / 8 + h / 3;
                    
                    if (rh > dh - 2)
                    {
                        rh = dh - 2;
                    }

                    int i = (x + y * width) * depth + z;
                    int id = 0;
                    
                    if (y == dh)
                    {
                        id = Block.grass.id;
                    }

                    if (y < dh)
                    {
                        id = Block.dirt.id;
                    }

                    if (y <= rh)
                    {
                        id = Block.rock.id;
                    }

                    this.blocks[i] = (byte)id;
                }
            }
        }
    }

    public bool Load()
    {
        try
        {
            /*
            using (FileStream fs = new FileStream("level.dat", FileMode.Open))
            using (GZipStream gzip = new GZipStream(fs, CompressionMode.Decompress))
            using (BinaryReader dis = new BinaryReader(gzip))
            {
                dis.Read(blocks, 0, blocks.Length);
                dis.Close();

                return true;
            }
            */
            blocks = File.ReadAllBytes("level.dat");

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);

            return false;
        }
    }
    
    public void Save()
    {
        try
        {
            /*
            using (FileStream fs = new FileStream("level.dat", FileMode.Create))
            using (GZipStream gzip = new GZipStream(fs, CompressionMode.Compress))
            using (BinaryWriter dos = new BinaryWriter(gzip))
            {
                dos.Write(blocks);
                dos.Close();
            }
            */
            File.WriteAllBytes("level.dat", blocks);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }
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
    
    public bool SetBlock(int x, int y, int z, int type)
    {
        if (x >= 0 && x < width &&
            y >= 0 && y < height &&
            z >= 0 && z < depth)
        {
            if (type == blocks[(x + y * width) * depth + z])
            {
                return false;
            }
            else
            {
                blocks[(x + y * width) * depth + z] = (byte)type;

                return true;
            }
        }

        return false;
    }

    public int GetBlock(int x, int y, int z)
    {
        if (x >= 0 && x < width &&
           y >= 0 && y < height &&
           z >= 0 && z < depth)
        {
            return blocks[(x + y * width) * depth + z];
        }

        return 0;
    }

    public bool IsSolidBlock(int x, int y, int z)
    {
        Block block = Block.blocks[GetBlock(x, y, z)];

        return block == null ? false : block.IsSolid();
    }
}
