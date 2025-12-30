using OpenTK.Mathematics;

namespace Minecraft;

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
        if (x1 > other.x0 && x0 < other.x0)
        {
            // Colisão pela esquerda, ajustar para fora à direita
            x += other.x0 - x1;
        }
        else if (x0 < other.x1 && x1 > other.x1)
        {
            // Colisão pela direita, ajustar para fora à esquerda
            x += other.x1 - x0;
        }
        
        return x;
    }
    
    public float ClipYCollide(AABB other, float y)
    {
        if (y1 > other.y0 && y0 < other.y0)
        {
            // Colisão por baixo (pisando no chão), ajustar para cima
            y += other.y0 - y1;
        }
        else if (y0 < other.y1 && y1 > other.y1)
        {
            // Colisão por cima (cabeça), ajustar para baixo
            y += other.y1 - y0;
        }
        
        return y;
    }
    
    public float ClipZCollide(AABB other, float z)
    {
        if (z1 > other.z0 && z0 < other.z0)
        {
            // Colisão por trás, ajustar para frente
            z += other.z0 - z1;
        }
        else if (z0 < other.z1 && z1 > other.z1)
        {
            // Colisão pela frente, ajustar para trás
            z += other.z1 - z0;
        }
        
        return z;
    }
}
