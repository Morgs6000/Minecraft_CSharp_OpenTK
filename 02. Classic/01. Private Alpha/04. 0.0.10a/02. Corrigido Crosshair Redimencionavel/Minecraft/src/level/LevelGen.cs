namespace Minecraft;

public class LevelGen
{
    private int width;
    private int height; // Agora associado ao eixo Y
    private int depth;  // Agora associado ao eixo Z

    private Random random = new Random();

    public LevelGen(int width, int height, int depth)
    {
        this.width = width;
        this.height = height;
        this.depth = depth;
    }

    public byte[] GenerateMap()
    {
        int w = this.width;
        int h = this.height; // Altura (Y)
        int d = this.depth;  // Profundidade (Z)

        // Mapas de ruído baseados no plano XZ
        int[] heightmap1 = new PerlinNoise(0).Read(w, d);
        int[] heightmap2 = new PerlinNoise(0).Read(w, d);
        int[] cf = new PerlinNoise(1).Read(w, d);
        int[] rockMap = new PerlinNoise(1).Read(w, d);

        byte[] blocks = new byte[width * height * depth];

        for (int x = 0; x < w; ++x)
        {
            for (int y = 0; y < h; ++y) // Loop de altura (Y)
            {
                for (int z = 0; z < d; ++z) // Loop de profundidade (Z)
                {
                    // Pré-calcula os valores de altura para esta coluna XZ
                    int dh1 = heightmap1[x + z * width];
                    int dh2 = heightmap2[x + z * width];

                    int cfh = cf[x + z * width];
                    if (cfh < 128) 
                    {
                        dh2 = dh1;
                    }

                    int dh = dh1;
                    if (dh2 > dh1) 
                    {
                        dh = dh2;
                    }

                    // Ajusta a escala da altura (dh)
                    dh = dh / 8 + h / 3;

                    // Ajusta o nível da rocha (rh)
                    int rh = rockMap[x + z * w] / 8 + h / 3;
                    if (rh > dh - 2) 
                    {
                        rh = dh - 2;
                    }
                
                    // Cálculo do índice 3D: (y * depth + z) * width + x ou similar
                    // Mantendo a estrutura de indexação linear compatível com sua lógica:
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

                    blocks[i] = (byte)id;
                }
            }
        }

        // --- Geração de Cavernas (Worms) ---
        int count = w * h * d / 256 / 64;

        for (int i = 0; i < count; ++i)
        {
            float x = (float)this.random.NextDouble() * w;
            float y = (float)this.random.NextDouble() * h; // Y = Altura
            float z = (float)this.random.NextDouble() * d; // Z = Profundidade

            int length = (int)((float)this.random.NextDouble() + (float)this.random.NextDouble() * 150.0f);

            float dir1 = (float)(this.random.NextDouble() * Math.PI * 2.0);
            float dira1 = 0.0f;
            float dir2 = (float)(this.random.NextDouble() * Math.PI * 2.0);
            float dira2 = 0.0f;

            for (int l = 0; l < length; ++l)
            {
                x = (float)(x + Math.Sin(dir1) * Math.Cos(dir2));
                z = (float)(z + Math.Cos(dir1) * Math.Cos(dir2)); // Movimento em Z
                y = (float)(y + Math.Sin(dir2));                 // Movimento em Y

                dir1 += dira1 * 0.2f;
                dira1 *= 0.9f;
                dira1 += (float)this.random.NextDouble() - (float)this.random.NextDouble();

                dir2 += dira2 * 0.5f;
                dir2 *= 0.5f;
                dira2 *= 0.9f;
                dira2 += (float)this.random.NextDouble() - (float)this.random.NextDouble();

                float size = (float)(Math.Sin(l * Math.PI / length) * 2.5 + 1.0);

                for (int xx = (int)(x - size); xx <= (int)(x + size); ++xx)
                {
                    for (int yy = (int)(y - size); yy <= (int)(y + size); ++yy) // YY para altura
                    {
                        for (int zz = (int)(z - size); zz <= (int)(z + size); ++zz) // ZZ para profundidade
                        {
                            if (xx >= 0 && yy >= 0 && zz >= 0 && xx < w && yy < h && zz < d)
                            {
                                float xd = xx - x;
                                float yd = yy - y;
                                float zd = zz - z;

                                // yd * 2.0f achata a caverna verticalmente (opcional)
                                float dd = xd * xd + yd * yd * 2.0f + zd * zd;

                                if (dd < size * size)
                                {
                                    int ii = (yy * d + zz) * w + xx;
                                    
                                    if (blocks[ii] == Block.rock.id)
                                    {
                                        blocks[ii] = 0; // Escava a rocha
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return blocks;
    }
}
