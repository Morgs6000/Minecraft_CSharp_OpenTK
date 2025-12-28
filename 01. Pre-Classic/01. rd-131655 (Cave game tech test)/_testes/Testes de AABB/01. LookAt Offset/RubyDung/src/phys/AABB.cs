using OpenTK.Mathematics;

namespace RubyDung.phys;

/* Axis-Aligned Bounding Box */

public class AABB
{
    public float x0;
    public float y0;
    public float z0;

    public float x1;
    public float y1;
    public float z1;

    public AABB(float x0, float y0, float z0, float x1, float y1, float z1)
    {
        this.x0 = x0;
        this.y0 = y0;
        this.z0 = z0;

        this.x1 = x1;
        this.y1 = y1;
        this.z1 = z1;
    }

    public bool Intersects(AABB other)
    {
        bool collisionX = x1 >= other.x0 && x0 <= other.x1;
        bool collisionY = y1 >= other.y0 && y0 <= other.y1;
        bool collisionZ = z1 >= other.z0 && z0 <= other.z1;

        return collisionX && collisionY && collisionZ;
    }
    
    public Vector3 CalculateOverlap(AABB other)
    {
        float overlapX = Math.Min(x1 - other.x0, other.x1 - x0);
        float overlapY = Math.Min(y1 - other.y0, other.y1 - y0);
        float overlapZ = Math.Min(z1 - other.z0, other.z1 - z0);

        return new Vector3(overlapX, overlapY, overlapZ);
    }
    
    public float ClipXCollide(AABB other, float x)
    {
        float width = x1 - x0; // Largura deste AABB
    
        if (x0 <= other.x1 && x1 > other.x1)
        {
            // Colisão pela direita, mover para fora à direita
            // x é a posição mínima (x0), então adicionamos a largura total
            x = other.x1;
        }
        if(x1 >= other.x0 && x0 < other.x0)
        {
            // Colisão pela esquerda, mover para fora à esquerda
            // x é a posição mínima (x0), então subtraímos a largura total
            x = other.x0 - width;
        }
        
        return x;
    }
    
    public float ClipYCollide(AABB other, float y)
    {
        float height = y1 - y0; // Altura deste AABB
    
        if (y0 <= other.y1 && y1 > other.y1)
        {
            // Colisão por cima, mover para fora acima
            // y é a posição mínima (y0), então adicionamos a altura total
            y = other.y1;
        }
        if(y1 >= other.y0 && y0 < other.y0)
        {
            // Colisão por baixo, mover para fora abaixo
            // y é a posição mínima (y0), então subtraímos a altura total
            y = other.y0 - height;
        }
        
        return y;
    }
    
    public float ClipZCollide(AABB other, float z)
    {
        float depth = z1 - z0; // Profundidade deste AABB
    
        if (z0 <= other.z1 && z1 > other.z1)
        {
            // Colisão pela frente, mover para fora à frente
            // z é a posição mínima (z0), então adicionamos a profundidade total
            z = other.z1;
        }
        if(z1 >= other.z0 && z0 < other.z0)
        {
            // Colisão por trás, mover para fora atrás
            // z é a posição mínima (z0), então subtraímos a profundidade total
            z = other.z0 - depth;
        }
        
        return z;
    }
}
