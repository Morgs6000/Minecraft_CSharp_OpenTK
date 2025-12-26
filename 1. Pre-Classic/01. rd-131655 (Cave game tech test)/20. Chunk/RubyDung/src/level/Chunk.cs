namespace RubyDung;

public class Chunk
{
    private Mesh mesh;

    public Chunk()
    {
        mesh = new Mesh();
    }

    public void Load()
    {
        mesh.Begin();

        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    Block.block.Load(mesh, x, y, z);
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
