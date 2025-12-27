using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RubyDung.common;

namespace RubyDung.level;

public class Highlight
{
    private Shader shader;

    private Level level;
    private Mesh mesh;
    private Camera camera;

    private HitResult hitResult = null!;

    public Highlight(Shader shader, Level level, Camera camera)
    {
        this.shader = shader;
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
    
    public void Draw(ShadedMode shadedMode)
    {
        mesh.Draw(shader, shadedMode);
    }

    private void RenderHit(HitResult h)
    {
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        GL.Enable(EnableCap.PolygonOffsetLine);
        GL.PolygonOffset(-1.0f, -1.0f);

        float alpha = (float)Math.Sin(Environment.TickCount / 100.0f) * 2.0f + 0.4f;
        shader.SetVector4("uniformColor", 1.0f, 1.0f, 1.0f, alpha);

        mesh.Begin();
        Block.rock.LoadFace(mesh, h.x, h.y, h.z);
        mesh.End();

        GL.Disable(EnableCap.PolygonOffsetLine);
        GL.Disable(EnableCap.Blend);
    }

    private bool Target()
    {
        // Posição e direção da camera
        Vector3 rayOrigem = camera.position;
        Vector3 rayDirection = camera.front;

        // Distancia maxima para verificar colisões
        float maxDistance = 5.0f;

        // Inicializa o algoritimo DDA (Digital Differential Analyzer)
        int mapX = (int)Math.Floor(rayOrigem.X);        
        int mapY = (int)Math.Floor(rayOrigem.Y);
        int mapZ = (int)Math.Floor(rayOrigem.Z);

        float deltaDistX = (float)Math.Abs(1 / rayDirection.X);
        float deltaDistY = (float)Math.Abs(1 / rayDirection.Y);
        float deltaDistZ = (float)Math.Abs(1 / rayDirection.Z);

        float sizeDistX;
        float sizeDistY;
        float sizeDistZ;

        int stepX;
        int stepY;
        int stepZ;

        // Determina a direção do passo e distancias laterias iniciais
        if (rayDirection.X < 0)
        {
            stepX = -1;
            sizeDistX = (rayOrigem.X - mapX) * deltaDistX;
        }
        else
        {
            stepX = 1;
            sizeDistX = (mapX + 1.0f - rayOrigem.X) * deltaDistX;
        }

        if (rayDirection.Y < 0)
        {
            stepY = -1;
            sizeDistY = (rayOrigem.Y - mapY) * deltaDistY;
        }
        else
        {
            stepY = 1;
            sizeDistY = (mapY + 1.0f - rayOrigem.Y) * deltaDistY;
        }

        if (rayDirection.Z < 0)
        {
            stepZ = -1;
            sizeDistZ = (rayOrigem.Z - mapZ) * deltaDistZ;
        }
        else
        {
            stepZ = 1;
            sizeDistZ = (mapZ + 1.0f - rayOrigem.Z) * deltaDistZ;
        }

        // Algoritimo DDA (Digital Differential Analyzer)
        int side = 0; // Qual lado foi atingido (0 = x, 1 = y, 2 = z)
        float distance = 0;

        while (distance < maxDistance)
        {
            // Pula para o proximo quadrado do grid
            if (sizeDistX < sizeDistY && sizeDistX < sizeDistZ)
            {
                sizeDistX += deltaDistX;
                mapX += stepX;
                side = 0;
            }
            if (sizeDistY < sizeDistX && sizeDistY < sizeDistZ)
            {
                sizeDistY += deltaDistY;
                mapY += stepY;
                side = 1;
            }
            if (sizeDistZ < sizeDistX && sizeDistZ < sizeDistY)
            {
                sizeDistZ += deltaDistZ;
                mapZ += stepZ;
                side = 2;
            }

            distance = (float)Math.Min(sizeDistX, Math.Min(sizeDistY, sizeDistZ));

            // Verifica se este bloco é sólido
            if (level.IsSolidBlock(mapX, mapY, mapZ))
            {
                hitResult.x = mapX;
                hitResult.y = mapY;
                hitResult.z = mapZ;

                // Determina qual face foi atingida baseado no lado e direção do passo
                if (side == 0)
                {
                    // hitResult.f = (stepX > 0) ? 0 : 1; // Oeste ou Leste
                }
                if (side == 1)
                {
                    // hitResult.f = (stepY > 0) ? 2 : 3; // Baixo ou Top
                }
                if (side == 2)
                {
                    // hitResult.f = (stepZ > 0) ? 4 : 5; // Norte ou Sul
                }

                return true;
            }
        }

        // hitResult.f = -1;

        return false;
    }
}
