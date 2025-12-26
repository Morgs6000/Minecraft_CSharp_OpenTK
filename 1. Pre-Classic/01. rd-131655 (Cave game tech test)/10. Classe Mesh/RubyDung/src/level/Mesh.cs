using OpenTK.Graphics.OpenGL4;

namespace RubyDung.Level;

public class Mesh
{
    private List<float> vertexBuffer = new List<float>();

    private uint VAO;
    private uint VBO;

    public void Begin()
    {
        Clear();
    }

    private void Clear()
    {
        vertexBuffer.Clear();
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
    }

    public void Draw()
    {
        GL.BindVertexArray(VAO);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
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
    }
}
