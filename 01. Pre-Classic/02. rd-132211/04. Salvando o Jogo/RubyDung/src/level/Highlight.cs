using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RubyDung.common;

namespace RubyDung.level;

public class Highlight
{
    private Level level;
    private Mesh mesh;
    private Terrain terrain;

    private Camera camera;
    private HitResult hitResult = null!;

    private Vector4 color;

    public Highlight(Level level, Terrain terrain, Camera camera)
    {
        this.level = level;
        this.terrain = terrain;
        this.camera = camera;

        mesh = new Mesh();

        hitResult = new HitResult(0, 0, 0, -1);
    }
    
    public void Update()
    {
        if (Target())
        {
            RenderHit(hitResult);

            int x = hitResult.x;
            int y = hitResult.y;
            int z = hitResult.z;

            if (Input.GetMouseButtonDown(1))
            {
                SetBlock(x, y, z, 0);
            }
            if (Input.GetMouseButtonDown(0))
            {   
                if(hitResult.f == 0)
                {
                    x--;
                }
                if(hitResult.f == 1)
                {
                    x++;
                }
                if(hitResult.f == 2)
                {
                    y--;
                }
                if(hitResult.f == 3)
                {
                    y++;
                }
                if(hitResult.f == 4)
                {
                    z--;
                }
                if (hitResult.f == 5)
                {
                    z++;
                }
                
                SetBlock(x, y, z, 1);
            }
        }
        else
        {
            mesh.Begin();
        }
    }
    
    public void Draw(Shader shader, ShadedMode shadedMode)
    {
        shader.SetBool("hasCustomColor", true);
        shader.SetVector4("uniformColor", color);

        mesh.Draw(shader, shadedMode);
    }

    private void RenderHit(HitResult h)
    {
        GL.DepthFunc(DepthFunction.Lequal);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        // float alpha = (float)((Math.Sin(Environment.TickCount / 100.0f) * 0.2f + 0.4f) * 0.5f);
        float alpha = (float)(Math.Sin(Environment.TickCount / 100.0f) * 0.2f + 0.4f);

        color = new Vector4(1.0f, 1.0f, 1.0f, alpha);
        // color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

        // Debug.Log($"alpha: {alpha:f1}");

        mesh.Begin();
        Block.rock.LoadFace(mesh, h.x, h.y, h.z, h.f);
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
        int side = 0;

        while (!hit && (sideDistX <= maxDistance || sideDistY <= maxDistance || sideDistZ <= maxDistance))
        {
            // Avança para o próximo quadrado do grid
            if (sideDistX < sideDistY)
            {
                if (sideDistX < sideDistZ)
                {
                    sideDistX += deltaDistX;
                    mapX += stepX;
                    side = 0;
                }
                else
                {
                    sideDistZ += deltaDistZ;
                    mapZ += stepZ;
                    side = 2;
                }
            }
            else
            {
                if (sideDistY < sideDistZ)
                {
                    sideDistY += deltaDistY;
                    mapY += stepY;
                    side = 1;
                }
                else
                {
                    sideDistZ += deltaDistZ;
                    mapZ += stepZ;
                    side = 2;
                }
            }

            // Verifica se este bloco é sólido
            if (level.IsSolidBlock(mapX, mapY, mapZ))
            {
                hit = true;
                hitResult.x = mapX;
                hitResult.y = mapY;
                hitResult.z = mapZ;

                // Determina qual face foi atingida baseado no lado e direção do passo
                if(side == 0)
                {
                    hitResult.f = (stepX > 0) ? 0 : 1; // Oeste ou Leste
                }
                if(side == 1)
                {
                    hitResult.f = (stepY > 0) ? 2 : 3; // Baixo ou Topo
                }
                if(side == 2)
                {
                    hitResult.f = (stepZ > 0) ? 4 : 5; // Norte ou Sul
                }
            }
        }

        return hit;
    }

    private void SetBlock(int x, int y, int z, int type)
    {
        level.SetBlock(x, y, z, type);

        /*
        int chunkX = x / 16;
        int chunkY = y / 16;
        int chunkZ = z / 16;

        terrain.ChunkReload(chunkX, chunkY, chunkZ);
        */

        int x0 = x - 1;
        int y0 = y - 1;
        int z0 = z - 1;

        int x1 = x + 1;
        int y1 = y + 1;
        int z1 = z + 1;

        terrain.SetChunk(x0, y0, z0, x1, y1, z1);
    }
}
