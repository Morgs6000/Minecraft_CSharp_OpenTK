using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Minecraft;

public class Highlight
{
    private Level level;
    private Terrain terrain;
    private Mesh mesh;
    private Camera camera;
    private HitResult hitResult;

    private Vector4 color;

    private int editMode = 0;

    public Highlight(Level level, Terrain terrain, Camera camera)
    {
        this.level = level;
        this.terrain = terrain;
        this.camera = camera;
        
        mesh = new Mesh();

        hitResult = new HitResult(0, 0, 0, -1);
    }

    public void Update(GameWindow gameWindow, DrawGUI drawGUI)
    {
        MouseState mouseState = gameWindow.MouseState;

        if (Target())
        {
            RenderHit(hitResult, editMode, drawGUI.paintTexture);            

            if (mouseState.IsButtonPressed(MouseButton.Left))
            {
                HandleMouseClick(drawGUI);
            }
            if (mouseState.IsButtonPressed(MouseButton.Right))
            {
                editMode = (editMode + 1) % 2;
            }
        }
        else
        {
            mesh.Begin();
        }
    }
    
    private void HandleMouseClick(DrawGUI drawGUI)
    {
        int x = hitResult.x;
        int y = hitResult.y;
        int z = hitResult.z;

        if (editMode == 0)
        {
            if (hitResult != null)
            {
                Block oldBlock = Block.blocks[level.GetBlock(x, y, z)];

                bool changed = level.SetBlock(x, y, z, 0);

                if (oldBlock != null && changed)
                {
                    SetBlock(x, y, z, 0);
                }
            }
        }
        else if (hitResult != null)
        {
            if (hitResult.f == 0)
            {
                x--;
            }
            if (hitResult.f == 1)
            {
                x++;
            }
            if (hitResult.f == 2)
            {
                y--;
            }
            if (hitResult.f == 3)
            {
                y++;
            }
            if (hitResult.f == 4)
            {
                z--;
            }
            if (hitResult.f == 5)
            {
                z++;
            }

            AABB blockAABB = new AABB(x, y, z, x + 1, y + 1, z + 1);

            if (camera.cameraBox.Intersects(blockAABB))
            {
                return;
            }

            SetBlock(x, y, z, drawGUI.paintTexture);
        }
    }

    public void Render(Shader shader)
    {
        shader.SetBool("hasUniformColor", true);
        shader.SetVector4("uniformColor", color);

        mesh.Render(shader);
    }

    private void RenderHit(HitResult h, int mode, int blockType)
    {
        GL.DepthFunc(DepthFunction.Lequal);

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        float alpha = (float)(Math.Sin(Environment.TickCount / 100.0f) * 0.2f + 0.4f);
        color = new Vector4(1.0f, 1.0f, 1.0f, alpha);

        if (mode == 0)
        {
            mesh.Begin();

            for (int i = 0; i < 6; i++)
            {
                Block.rock.LoadFaceNoTexture(mesh, h.x, h.y, h.z, i);
            }

            mesh.End();
        }
        else
        {
            float br = (float)(Math.Sin(Environment.TickCount / 100.0f) * 0.2f + 0.8f);
            float alpha2 = (float)(Math.Sin(Environment.TickCount / 200.0f) * 0.2f + 0.5f);
            color = new Vector4(br, br, br, alpha2);

            int x = h.x;
            int y = h.y;
            int z = h.z;

            if(h.f == 0)
            {
                x--;
            }
            if(h.f == 1)
            {
                x++;
            }
            if(h.f == 2)
            {
                y--;
            }
            if(h.f == 3)
            {
                y++;
            }
            if(h.f == 4)
            {
                z--;
            }
            if (h.f == 5)
            {
                z++;
            }

            mesh.Begin();

            mesh.NoColor();

            Block.blocks[blockType].Load(mesh, level, x, y, z);

            mesh.End();
        }       

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
            if (level.GetBlock(mapX, mapY, mapZ) != 0)
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

        int x0 = x - 1;
        int y0 = y - 1;
        int z0 = z - 1;

        int x1 = x + 1;
        int y1 = y + 1;
        int z1 = z + 1;

        terrain.SetChunk(x0, y0, z0, x1, y1, z1);
    }
}
