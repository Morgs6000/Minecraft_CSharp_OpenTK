namespace RubyDung;

public class Chunk
{
    private Level level;
    private Mesh mesh;

    public readonly int x0;
    public readonly int y0;
    public readonly int z0;

    public readonly int x1;
    public readonly int y1;
    public readonly int z1;

    public Chunk(Level level, int x0, int y0, int z0, int x1, int y1, int z1)
    {
        this.level = level;

        this.x0 = x0;
        this.y0 = y0;
        this.z0 = z0;

        this.x1 = x1;
        this.y1 = y1;
        this.z1 = z1;

        mesh = new Mesh();
    }

    public void Load()
    {
        mesh.Begin();

        for (int x = x0; x < x1; x++)
        {
            for (int y = y0; y < y1; y++)
            {
                for (int z = z0; z < z1; z++)
                {
                    if (level.IsBlock(x, y, z))
                    {
                        int tex = y == level.height * 2 / 3 ? 1 : 0;

                        if (tex == 1)
                        {
                            Block.grass.Load(mesh, level, x, y, z);
                        }
                        else
                        {
                            Block.rock.Load(mesh, level, x, y, z);
                        }
                    }
                }                
            }            
        }        

        mesh.End();
    }

    public void Draw(Shader shader, ShadedMode shadedMode)
    {
        if (shadedMode == ShadedMode.Shaded || shadedMode == ShadedMode.ShadedWireframe)
        {
            shader.SetBool("hasWireframe", false);
            mesh.Draw(shader);
        }
        if (shadedMode == ShadedMode.Wireframe || shadedMode == ShadedMode.ShadedWireframe)
        {
            shader.SetBool("hasWireframe", true);
            mesh.DrawWireframe();
        }  
    }
}
