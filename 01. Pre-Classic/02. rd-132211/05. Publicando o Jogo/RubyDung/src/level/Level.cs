using System.IO.Compression;
using RubyDung.common;
using RubyDung.phys;

namespace RubyDung.level;

public class Level
{
    public readonly int width;
    public readonly int height;
    public readonly int depth;

    // private byte[] blocks;
    private byte[,,] blocks;

    public Level(int w, int h, int d)
    {
        width = w;
        height = h;
        depth = d;

        // blocks = new byte[w * h * d];
        blocks = new byte[w, h, d];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                for (int z = 0; z < d; z++)
                {
                    // int i = (x + y * width) * depth + z;
                    // blocks[i] = (byte)(y <= h * 2 / 3 ? 1 : 0);
                    blocks[x, y, z] = (byte)(y <= h * 2 / 3 ? 1 : 0);
                }
            }
        }

        Load();
    }

    public void Load()
    {
        try
        {
            /*
            string filePath = "level.dat";

            if (!File.Exists(filePath))
            {
                Debug.LogWarning("Arquivo level.dat não encontrado. Criando novo nível.");
                return;
            }
            */
            string file = "level.dat";
            string filePath = $"save/{file}";
            string directoryPath = Path.GetDirectoryName(filePath);

            // Cria o diretório se não existir
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Debug.Log($"Diretório criado: {directoryPath}");
            }
            if (!File.Exists(filePath))
            {
                Debug.LogWarning("Arquivo 'level.dat' não encontrado. Criando novo nível.");
                return;
            }
            
            using(FileStream fs = new FileStream(filePath, FileMode.Open))
            using(GZipStream gzip = new GZipStream(fs, CompressionMode.Decompress))
            using (BinaryReader dis = new BinaryReader(gzip))
            {
                // dis.Read(blocks, 0, blocks.Length);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            blocks[x, y, z] = dis.ReadByte();
                        }
                    }
                }

                dis.Close();

                Debug.LogSuccess("Nível carregado com sucesso!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao carregar nível: {e.Message}");
            Debug.LogError(e.StackTrace);
            throw;
        }
    }

    public void Save()
    {
        try
        {
            // string filePath = "level.dat";

            string file = "level.dat";
            string filePath = $"save/{file}";
            string directoryPath = Path.GetDirectoryName(filePath);

            // Cria o diretório se não existir
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Debug.Log($"Diretório criado: {directoryPath}");
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            using (GZipStream gzip = new GZipStream(fs, CompressionMode.Compress))
            using (BinaryWriter dos = new BinaryWriter(gzip))
            {
                // dos.Write(blocks);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            dos.Write(blocks[x, y, z]);
                        }
                    }
                }

                dos.Close();

                Debug.LogSuccess("Nível salvo com sucesso!");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao salvar nível: {e.Message}");
            Debug.LogError(e.StackTrace);
            throw;
        }
    }

    public bool IsBlock(int x, int y, int z)
    {
        if (x >= 0 && x < width &&
            y >= 0 && y < height &&
            z >= 0 && z < depth)
        {
            // return blocks[(x + y * width) * depth + z] == 1;
            return blocks[x, y, z] == 1;
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

        for (int x = x0; x < x1; ++x)
        {
            for (int y = y0; y < y1; ++y)
            {
                for (int z = z0; z < z1; ++z)
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
    
    public void SetBlock(int x, int y, int z, int type)
    {
        if (x >= 0 && x < width &&
            y >= 0 && y < height &&
            z >= 0 && z < depth)
        {
            // blocks[(x + y * width) * depth + z] = (byte)type;
            blocks[x, y, z] = (byte)type;
        }
    }
}
