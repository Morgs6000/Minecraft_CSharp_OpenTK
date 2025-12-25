using OpenTK.Graphics.OpenGL4;

namespace RubyDung;

public class Mesh
{
    private List<float> vertexBuffer = new List<float>();
    private List<uint> indiceBuffer = new List<uint>();
    private List<float> colorBuffer = new List<float>();

    private uint vertices = 0;

    private float r;
    private float g;
    private float b;

    private bool hasColor = false;

    private uint VAO;
    private uint VBO;
    private uint EBO;
    private uint CBO;

    public void Begin()
    {
        Clear();

        hasColor = false;
    }
    
    private void Clear()
    {
        vertexBuffer.Clear();
        indiceBuffer.Clear();
        colorBuffer.Clear();

        vertices = 0;
    }

    public void End()
    {
        GL.GenVertexArrays(1, out VAO);
        GL.BindVertexArray(VAO);

        GL.GenBuffers(1, out VBO);
        GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, vertexBuffer.Count * sizeof(float), vertexBuffer.ToArray(), BufferUsageHint.StaticDraw);

        int aPosition = 0;
        GL.VertexAttribPointer(aPosition, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(aPosition);

        GL.GenBuffers(1, out EBO);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indiceBuffer.Count * sizeof(uint), indiceBuffer.ToArray(), BufferUsageHint.StaticDraw);

        if (hasColor)
        {
            GL.GenBuffers(1, out CBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, CBO);
            GL.BufferData(BufferTarget.ArrayBuffer, colorBuffer.Count * sizeof(float), colorBuffer.ToArray(), BufferUsageHint.StaticDraw);

            int aColor = 1;
            GL.VertexAttribPointer(aColor, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(aColor);
        }
    }

    public void Draw(Shader shader)
    {
        shader.SetBool("hasColor", hasColor);

        GL.BindVertexArray(VAO);

        GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
        
        GL.DrawElements(PrimitiveType.Triangles, indiceBuffer.Count, DrawElementsType.UnsignedInt, 0);

        GL.BindVertexArray(0);
    }

    public void DrawWireframe()
    {
        GL.BindVertexArray(VAO);

        // Configura o offset para o wireframe
        GL.Enable(EnableCap.PolygonOffsetLine);
        GL.PolygonOffset(-1.0f, -1.0f);  // Valores negativos para "puxar" o wireframe para frente

        GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
        GL.LineWidth(1.0f);

        GL.DrawElements(PrimitiveType.Triangles, indiceBuffer.Count, DrawElementsType.UnsignedInt, 0);
        
        // Desativa o offset após desenhar
        GL.Disable(EnableCap.PolygonOffsetLine);

        GL.BindVertexArray(0);
    }

    public void Vertex2(float x, float y)
    {
        Vertex3(x, y, 0.0f);
    }

    public void Vertex3(float x, float y, float z)
    {
        vertexBuffer.Add(x);
        vertexBuffer.Add(y);
        vertexBuffer.Add(z);

        vertices++;

        Indice();

        if (hasColor)
        {
            colorBuffer.Add(r);
            colorBuffer.Add(g);
            colorBuffer.Add(b);
        }
    }

    private void Indice()
    {
        if (vertices % 4 == 0)
        {
            uint indices = vertices - 4;

            // Primeiro Triangulo
            indiceBuffer.Add(0 + indices);
            indiceBuffer.Add(1 + indices);
            indiceBuffer.Add(2 + indices);

            // Segundo Triangulo
            indiceBuffer.Add(0 + indices);
            indiceBuffer.Add(2 + indices);
            indiceBuffer.Add(3 + indices);
        }
    }

    public void Color(float r, float g, float b)
    {
        hasColor = true;

        this.r = r;
        this.g = g;
        this.b = b;
    }
}
