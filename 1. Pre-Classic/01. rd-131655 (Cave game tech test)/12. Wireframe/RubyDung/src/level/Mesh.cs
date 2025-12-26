using OpenTK.Graphics.OpenGL4;
using RubyDung.Common;

namespace RubyDung.Level;

public class Mesh
{
    private List<float> vertexBuffer = new List<float>();
    private List<uint> indiceBuffer = new List<uint>();

    private uint vertices = 0;

    private uint VAO;
    private uint VBO;
    private uint EBO;

    public void Begin()
    {
        Clear();
    }

    private void Clear()
    {
        vertexBuffer.Clear();
        indiceBuffer.Clear();

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
    }

    public void Draw(Shader shader, ShadedMode shadedMode)
    {
        GL.BindVertexArray(VAO);

        if (shadedMode == ShadedMode.Shaded || shadedMode == ShadedMode.ShadedWireframe)
        {
            shader.SetBool("hasWireframe", false);
            DrawFill();
        }
        if (shadedMode == ShadedMode.Wireframe || shadedMode == ShadedMode.ShadedWireframe)
        {
            shader.SetBool("hasWireframe", true);
            DrawWireframe();
        }  

        GL.BindVertexArray(0);
    }
    
    private void DrawFill()
    {
        GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);

        GL.DrawElements(PrimitiveType.Triangles, indiceBuffer.Count, DrawElementsType.UnsignedInt, 0);
    }

    private void DrawWireframe()
    {
        // Configura o offset para o wireframe
        GL.Enable(EnableCap.PolygonOffsetLine);
        GL.PolygonOffset(-1.0f, -1.0f);  // Valores negativos para "puxar" o wireframe para frente

        GL.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
        GL.LineWidth(1.0f);

        GL.DrawElements(PrimitiveType.Triangles, indiceBuffer.Count, DrawElementsType.UnsignedInt, 0);
        
        // Desativa o offset após desenhar
        GL.Disable(EnableCap.PolygonOffsetLine);
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
}
