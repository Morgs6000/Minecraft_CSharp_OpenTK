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

    public Highlight(Level level, Camera camera)
    {
        this.level = level;
        this.camera = camera;

        string vertexPath = "src/shaders/highlight/vertex.glsl";
        string fragmentPath = "src/shaders/highlight/fragment.glsl";
        shader = new Shader(vertexPath, fragmentPath);

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
        shader.Use();

        Matrix4 model = Matrix4.Identity;
        shader.SetMatrix4("model", model);

        Matrix4 view = Matrix4.Identity;
        view *= camera.LookAt();
        shader.SetMatrix4("view", view);

        Matrix4 projection = Matrix4.Identity;
        projection *= camera.CreatePerspectiveFieldOfView();
        shader.SetMatrix4("projection", projection);

        mesh.Draw(shader, shadedMode);
    }

    private void RenderHit(HitResult h)
    {
        GL.DepthFunc(DepthFunction.Lequal);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        float alpha = (float)Math.Sin(Environment.TickCount / 100.0f) * 2.0f + 0.4f;
        shader.SetVector4("uniformColor", 1.0f, 1.0f, 1.0f, alpha);

        mesh.Begin();
        Block.rock.LoadFace(mesh, h.x, h.y, h.z);
        mesh.End();

        // GL.Disable(EnableCap.Blend);
    }

    private bool Target()
    {
        // Posição e direção da camera
        Vector3 rayOrigem = camera.position;
        Vector3 rayDirection = camera.front;

        // Tamanho do passo para o raycast
        float step = 0.1f;

        // Distancia maxima para verificar colisões
        float maxDistance = 5.0f;

        Vector3 currentPos = rayOrigem;

        for (float distance = 0; distance < maxDistance; distance += step)
        {
            currentPos += rayDirection * step;

            hitResult.x = (int)Math.Floor(currentPos.X);
            hitResult.y = (int)Math.Floor(currentPos.Y);
            hitResult.z = (int)Math.Floor(currentPos.Z);

            // Verifica se há um bloco sólido nessa posição
            if (level.IsSolidBlock(hitResult.x, hitResult.y, hitResult.z))
            {
                return true;
            }
        }

        return false;
    }
}
