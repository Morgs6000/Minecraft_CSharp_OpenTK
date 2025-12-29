using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace RubyDung;

public class Highlight
{
    private Level level;
    private Mesh mesh;
    private Camera camera;
    private HitResult hitResult;
    
    private Vector4 color;

    public Highlight(Level level, Camera camera)
    {
        this.level = level;
        this.camera = camera;
        
        mesh = new Mesh();

        hitResult = new HitResult(0, 0, 0);
    }

    public void Update()
    {
        if (Target())
        {
            RenderHit(hitResult);
        }
        else
        {
            mesh.Begin();
        }
    }

    public void Draw(Shader shader)
    {
        shader.SetBool("hasUniformColor", true);
        shader.SetVector4("uniformColor", color);

        mesh.Draw(shader);
    }

    private void RenderHit(HitResult h)
    {
        GL.DepthFunc(DepthFunction.Lequal);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        float alpha = (float)(Math.Sin(Environment.TickCount / 100.0f) * 0.2f + 0.4f);
        color = new Vector4(1.0f, 1.0f, 1.0f, alpha);

        mesh.Begin();

        Block.rock.LoadFace(mesh, h.x, h.y, h.z, 0);
        Block.rock.LoadFace(mesh, h.x, h.y, h.z, 1);
        Block.rock.LoadFace(mesh, h.x, h.y, h.z, 2);
        Block.rock.LoadFace(mesh, h.x, h.y, h.z, 3);
        Block.rock.LoadFace(mesh, h.x, h.y, h.z, 4);
        Block.rock.LoadFace(mesh, h.x, h.y, h.z, 5);

        mesh.End();

        // GL.Disable(EnableCap.Blend);
    }
    
    private bool Target()
    {
        // Posição e direção da camera
        Vector3 rayOrigem = camera.position;
        Vector3 rayDirection = camera.front;

        // Distancia maxima para verificar colisões
        float maxDistance = 5.0f;

        /* Digital Differential Analyzer) */

        // Inicializa o algoritimo DDA
        int mapX = (int)Math.Floor(rayOrigem.X);
        int mapY = (int)Math.Floor(rayOrigem.Y);
        int mapZ = (int)Math.Floor(rayOrigem.Z);

        // Verifica se já está dentro de um bloco sólido (raro, mas pode acontecer)
        if (level.IsSolidBlock(mapX, mapY, mapZ))
        {
            hitResult.x = mapX;
            hitResult.y = mapY;
            hitResult.z = mapZ;
            return true;
        }

        float deltaDistX = (rayDirection.X == 0) ? float.MaxValue : (float)Math.Abs(1 / rayDirection.X);
        float deltaDistY = (rayDirection.Y == 0) ? float.MaxValue : (float)Math.Abs(1 / rayDirection.Y);
        float deltaDistZ = (rayDirection.Z == 0) ? float.MaxValue : (float)Math.Abs(1 / rayDirection.Z);

        // float perpWallDist;
        float sideDistX;
        float sideDistY;
        float sideDistZ;

        int stepX;
        int stepY;
        int stepZ;

        // Determina a direção do passo e distancias laterais iniciais
        if (rayDirection.X < 0)
        {
            stepX = -1;
            sideDistX = (rayOrigem.X - mapX) * deltaDistX;
        }
        else
        {
            stepX = 1;
            sideDistX = (mapX + 1.0f - rayOrigem.X) * deltaDistX;
        }

        if (rayDirection.Y < 0)
        {
            stepY = -1;
            sideDistY = (rayOrigem.Y - mapY) * deltaDistY;
        }
        else
        {
            stepY = 1;
            sideDistY = (mapY + 1.0f - rayOrigem.Y) * deltaDistY;
        }

        if (rayDirection.Z < 0)
        {
            stepZ = -1;
            sideDistZ = (rayOrigem.Z - mapZ) * deltaDistZ;
        }
        else
        {
            stepZ = 1;
            sideDistZ = (mapZ + 1.0f - rayOrigem.Z) * deltaDistZ;
        }

        // Algoritimo DDA
        bool hit = false;

        while (!hit && (sideDistX <= maxDistance || sideDistY <= maxDistance || sideDistZ <= maxDistance))
        {
            // Avança para o próximo quadrado do grid
            if (sideDistX < sideDistY)
            {
                if (sideDistX < sideDistZ)
                {
                    sideDistX += deltaDistX;
                    mapX += stepX;
                }
                else
                {
                    sideDistZ += deltaDistZ;
                    mapZ += stepZ;
                }
            }
            else
            {
                if (sideDistY < sideDistZ)
                {
                    sideDistY += deltaDistY;
                    mapY += stepY;
                }
                else
                {
                    sideDistZ += deltaDistZ;
                    mapZ += stepZ;
                }
            }

            // Verifica se este bloco é sólido
            if (level.IsSolidBlock(mapX, mapY, mapZ))
            {
                hit = true;
                hitResult.x = mapX;
                hitResult.y = mapY;
                hitResult.z = mapZ;
            }
        }

        return hit;
    }
}
