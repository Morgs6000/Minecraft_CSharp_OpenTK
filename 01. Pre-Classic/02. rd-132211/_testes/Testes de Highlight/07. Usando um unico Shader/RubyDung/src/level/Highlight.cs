using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RubyDung.common;

namespace RubyDung.level;

public class Highlight
{
    // private Shader shader;

    private Level level;
    private Mesh mesh;
    private Camera camera;

    private HitResult hitResult = null!;

    private Vector4 color;

    public Highlight(Level level, Camera camera)
    {
        this.level = level;
        this.camera = camera;

        // string vertexPath = "src/shaders/highlight/vertex.glsl";
        // string fragmentPath = "src/shaders/highlight/fragment.glsl";
        // shader = new Shader(vertexPath, fragmentPath);
        // this.shader = shader;

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
    
    public void Draw(Shader shader, ShadedMode shadedMode)
    {
        // shader.Use();

        // Matrix4 model = Matrix4.Identity;
        // shader.SetMatrix4("model", model);

        // Matrix4 view = Matrix4.Identity;
        // view *= camera.LookAt();
        // shader.SetMatrix4("view", view);

        // Matrix4 projection = Matrix4.Identity;
        // projection *= camera.CreatePerspectiveFieldOfView();
        // shader.SetMatrix4("projection", projection);

        shader.SetBool("hasCustomColor", true);
        shader.SetVector4("uniformColor", color);

        mesh.Draw(shader, shadedMode);
    }

    private void RenderHit(HitResult h)
    {
        GL.DepthFunc(DepthFunction.Lequal);
        // GL.Disable(EnableCap.DepthTest);
        // GL.Disable(EnableCap.CullFace);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        // GL.BlendEquation(BlendEquationMode.FuncAdd);

        // GL.Enable(EnableCap.PolygonOffsetFill);
        // GL.PolygonOffset(-1.0f, -1.0f);

        // float alpha = (float)((Math.Sin(Environment.TickCount / 100.0f) * 0.2f + 0.4f) * 0.5f);
        float alpha = (float)(Math.Sin(Environment.TickCount / 100.0f) * 0.2f + 0.4f);

        color = new Vector4(1.0f, 1.0f, 1.0f, alpha);
        // color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

        // Debug.Log($"alpha: {alpha:f1}");

        mesh.Begin();
        Block.rock.LoadFace(mesh, h.x, h.y, h.z);
        mesh.End();

        // GL.Disable(EnableCap.PolygonOffsetFill);
        // GL.Disable(EnableCap.Blend);

        // GL.Enable(EnableCap.CullFace);
        // GL.Enable(EnableCap.DepthTest);
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

        float perpWallDist;
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
            }
        }

        return hit;
    }
}
