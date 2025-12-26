using OpenTK.Mathematics;
using RubyDung.phys;

namespace RubyDung;

public class Camera
{
    private AABB cameraBox;
    private AABB blockBox;

    private Vector3 Position = new Vector3(0.0f, 0.0f, 3.0f);
    private Vector3 Front    = new Vector3(0.0f, 0.0f, -1.0f);
    private Vector3 Up       = new Vector3(0.0f, 1.0f, 0.0f);

    private float MovementSpeed;
    
    private float walking      = 4.317f;
    private float sprinting    = 5.612f;
    private float sneaking     = 1.295f;
    private float flying       = 10.79f;
    private float sprintFlying = 21.58f;

    private float falling      = 77.71f;
    private float jumping      = 1.2522f;

    private bool hasFly = true;
    private bool hasCollision = true;

    private bool fistMouse = true;
    private Vector2 last;

    private float pitch;        // rotX // inclinação
    private float yaw = -90.0f; // rotY // guinada
    // private float roll;         // rotZ // rolamento

    private float playerWidht  = 0.6f;
    private float playerHeight = 1.8f;
    private float eyeHeight    = 1.62f;
    
    public Camera()
    {
        if (!hasFly)
        {
            MovementSpeed = walking;
        }
        else
        {
            MovementSpeed = flying;
        }
    }
    
    public void Update()
    {
        ProcessKeyboard();
        ProcessMouseMovement();
        // ProcessMouseScroll();

        if (hasCollision)
        {
            CheckCollisions();
        }
        
    }

    private void ProcessKeyboard()
    {
        Movement();
        Sprinting();
    }
    
    private void Movement()
    {
        float speed = MovementSpeed * Time.deltaTime;

        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        if (Input.GetKey(KeyCode.W))
        {
            z++;
        }
        if (Input.GetKey(KeyCode.S))
        {
            z--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            x++;
        }
        if (Input.GetKey(KeyCode.A))
        {
            x--;
        }
        if (!hasFly)
        {
            if (Input.GetKey(KeyCode.Space))
            {

            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                y++;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                y--;
            }
        }

        Position += x * speed * Vector3.Normalize(Vector3.Cross(Front, Up));
        Position += y * speed * Up;
        Position += z * speed * Vector3.Normalize(new Vector3(Front.X, 0.0f, Front.Z));
    }

    private void Sprinting()
    {
        if (Input.GetKeyDouble(KeyCode.W))
        {
            if (!hasFly)
            {
                MovementSpeed = sprinting;
            }
            else
            {
                MovementSpeed = sprintFlying;
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (!hasFly)
                {
                    MovementSpeed = sprinting;
                }
                else
                {
                    MovementSpeed = sprintFlying;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (!hasFly)
            {
                MovementSpeed = walking;
            }
            else
            {
                MovementSpeed = flying;
            }
        }

        // Debug.Log(speed);
    }

    private void ProcessMouseMovement()
    {
        if (fistMouse)
        {
            last.X = Input.mousePosition.X;
            last.Y = Input.mousePosition.Y;

            fistMouse = false;
        }

        float xoffset = Input.mousePosition.X - last.X;
        float yoffset = last.Y - Input.mousePosition.Y;
        last.X = Input.mousePosition.X;
        last.Y = Input.mousePosition.Y;

        const float sensitivity = 0.1f;
        xoffset *= sensitivity;
        yoffset *= sensitivity;

        yaw += xoffset;
        pitch += yoffset;
        
        Math.Clamp(pitch, -89.0f, 89.0f);

        Vector3 direction;
        direction.X = (float)(Math.Cos(MathHelper.DegreesToRadians(pitch)) * Math.Cos(MathHelper.DegreesToRadians(yaw)));
        direction.Y = (float)(Math.Sin(MathHelper.DegreesToRadians(pitch)));
        direction.Z = (float)(Math.Cos(MathHelper.DegreesToRadians(pitch)) * Math.Sin(MathHelper.DegreesToRadians(yaw)));

        Front = Vector3.Normalize(direction);
    }

    // private void ProcessMouseScroll()
    // {

    // }

    private void CheckCollisions()
    {
        UpdateCameraBox();
        UpdateBlockBox();

        // Verifica colisão AABB (Axis-Aligned Bounding Box)
        bool collisionX = cameraBox.x1 >= blockBox.x0 && cameraBox.x0 <= blockBox.x1;
        bool collisionY = cameraBox.y1 >= blockBox.y0 && cameraBox.y0 <= blockBox.y1;
        bool collisionZ = cameraBox.z1 >= blockBox.z0 && cameraBox.z0 <= blockBox.z1;

        // Se houver colisão em todos os eixos
        if (collisionX && collisionY && collisionZ)
        {
            string collisionFace = "";

            // Calcula as sobreposições
            float overlapX = Math.Min(cameraBox.x1 - blockBox.x0, blockBox.x1 - cameraBox.x0);
            float overlapY = Math.Min(cameraBox.y1 - blockBox.y0, blockBox.y1 - cameraBox.y0);
            float overlapZ = Math.Min(cameraBox.z1 - blockBox.z0, blockBox.z1 - cameraBox.z0);

            // Verifica qual é a menor sobreposição para resolver
            if (overlapX < overlapY && overlapX < overlapZ)
            {
                // Colisão em X
                float result = ClipXCollide(blockBox);
                Position.X += result;
                collisionFace = result < 0 ? "FACE POSITIVA EM X (leste)" : "FACE NEGATIVA EM X (oeste)";
            }
            else if (overlapY < overlapZ)
            {
                // Colisão em Y
                float result = ClipYCollide(blockBox);
                Position.Y += result;
                collisionFace = result < 0 ? "FACE POSITIVA EM Y (topo)" : "FACE NEGATIVA EM Y (base)";
            }
            else
            {
                // Colisão em Z
                float result = ClipZCollide(blockBox);
                Position.Z += result;
                collisionFace = result < 0 ? "FACE POSITIVA EM Z (norte)" : "FACE NEGATIVA EM Z (sul)";
            }

            Debug.Log($"Colisão na: {collisionFace}");

            // Atualiza a caixa da câmera após resolver a colisão
            UpdateCameraBox();
        }
    }

    private float ClipXCollide(AABB other)
    {
        // Calcula overlap X
        float overlapX = Math.Min(cameraBox.x1 - other.x0, other.x1 - cameraBox.x0);
        
        // Retorna o valor negativo se colidindo na face positiva de X
        // Retorna o valor positivo se colidindo na face negativa de X
        if (cameraBox.x1 > other.x0 && cameraBox.x1 - other.x0 == overlapX)
        {
            return -overlapX; // Face positiva em X (leste)
        }
        else
        {
            return overlapX; // Face negativa em X (oeste)
        }
    }
    
    private float ClipYCollide(AABB other)
    {
        // Calcula overlap Y
        float overlapY = Math.Min(cameraBox.y1 - other.y0, other.y1 - cameraBox.y0);
        
        // Retorna o valor negativo se colidindo na face positiva de Y
        // Retorna o valor positivo se colidindo na face negativa de Y
        if (cameraBox.y1 > other.y0 && cameraBox.y1 - other.y0 == overlapY)
        {
            return -overlapY; // Face positiva em Y (topo)
        }
        else
        {
            return overlapY; // Face negativa em Y (base)
        }
    }

    private float ClipZCollide(AABB other)
    {
        // Calcula overlap Z
        float overlapZ = Math.Min(cameraBox.z1 - other.z0, other.z1 - cameraBox.z0);
        
        // Retorna o valor negativo se colidindo na face positiva de Z
        // Retorna o valor positivo se colidindo na face negativa de Z
        if (cameraBox.z1 > other.z0 && cameraBox.z1 - other.z0 == overlapZ)
        {
            return -overlapZ; // Face positiva em Z (norte)
        }
        else
        {
            return overlapZ; // Face negativa em Z (sul)
        }
    }

    private void UpdateCameraBox()
    {
        float x0 = Position.X - (playerWidht / 2.0f);
        float y0 = Position.Y - (playerHeight / 2.0f);
        float z0 = Position.Z - (playerWidht / 2.0f);

        float x1 = Position.X + (playerWidht / 2.0f);
        float y1 = Position.Y + (playerHeight / 2.0f);
        float z1 = Position.Z + (playerWidht / 2.0f);

        cameraBox = new AABB(x0, y0, z0, x1, y1, z1);
    }
    
    private void UpdateBlockBox()
    {
        float x0 = 0.0f;
        float y0 = 0.0f;
        float z0 = 0.0f;

        float x1 = x0 + 1.0f;
        float y1 = y0 + 1.0f;
        float z1 = z0 + 1.0f;

        blockBox = new AABB(x0, y0, z0, x1, y1, z1);
    }

    public Matrix4 LookAt()
    {
        Vector3 eye    = Position;
        Vector3 target = Position + Front;
        Vector3 up     = Up;

        return Matrix4.LookAt(eye, target, up);
    }
}
