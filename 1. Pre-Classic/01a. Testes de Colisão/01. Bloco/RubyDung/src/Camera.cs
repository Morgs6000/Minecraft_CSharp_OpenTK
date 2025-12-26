using OpenTK.Mathematics;

namespace RubyDung;

public class Camera
{
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
        /*
        Vector3 playerMin = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 playerMax = new Vector3(playerWidht, playerHeight, playerWidht);

        Vector3 blockMin = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 blockMax = new Vector3(1.0f, 1.0f, 1.0f);
        */

        // Define a posição do bloco (z = -0.5f)
        Vector3 blockPos = new Vector3(0.0f, 0.0f, 0.0f);

        // Ajusta a posição do jogador para o centro
        Vector3 playerCenterPos = new Vector3(
            Position.X + (playerWidht / 2.0f),
            Position.Y - eyeHeight + (playerHeight / 2.0f), // Centro vertical do jogador
            Position.Z + (playerWidht / 2.0f)
        );

        // Define as caixas de colisão
        Vector3 playerMin = new Vector3(
            playerCenterPos.X,
            playerCenterPos.Y,
            playerCenterPos.Z
        );

        Vector3 playerMax = new Vector3(
            playerCenterPos.X + playerWidht,
            playerCenterPos.Y + playerHeight,
            playerCenterPos.Z + playerWidht
        );

        Vector3 blockMin = new Vector3(
            blockPos.X,
            blockPos.Y,
            blockPos.Z
        );

        Vector3 blockMax = new Vector3(
            blockPos.X + 1.0f,
            blockPos.Y + 1.0f,
            blockPos.Z + 1.0f
        );

        // Verifica colisão AABB (Axis-Aligned Bounding Box)
        bool collisionX = playerMax.X >= blockMin.X && playerMin.X <= blockMax.X;
        bool collisionY = playerMax.Y >= blockMin.Y && playerMin.Y <= blockMax.Y;
        bool collisionZ = playerMax.Z >= blockMin.Z && playerMin.Z <= blockMax.Z;

        // Se houver colisão em todos os eixos
        if (collisionX && collisionY && collisionZ)
        {
            Debug.Log("COLISÃO DETECTADA com face negativa em Z!");

            // Resolve a colisão empurrando o jogador para fora
            // Calcula a sobreposição em cada eixo
            float overlapX = Math.Min(playerMax.X - blockMin.X, blockMax.X - playerMin.X);
            float overlapY = Math.Min(playerMax.Y - blockMin.Y, blockMax.Y - playerMin.Y);
            float overlapZ = Math.Min(playerMax.Z - blockMin.Z, blockMax.Z - playerMin.Z);

            // Empurra pelo menor eixo de sobreposição
            if (overlapX < overlapY && overlapX < overlapZ)
            {
                // Colisão em X
                if (playerCenterPos.X < blockPos.X)
                    Position.X -= overlapX;
                else
                    Position.X += overlapX;
            }
            else if (overlapY < overlapZ)
            {
                // Colisão em Y
                if (playerCenterPos.Y < blockPos.Y)
                    Position.Y -= overlapY;
                else
                    Position.Y += overlapY;
            }
            else
            {
                // Colisão em Z (nosso foco!)
                if (playerCenterPos.Z < blockPos.Z)
                    Position.Z -= overlapZ;
                else
                    Position.Z += overlapZ;

                Debug.Log($"Colisão resolvida em Z! Posição Z ajustada para: {Position.Z}");
            }
        }
    }

    public Matrix4 LookAt()
    {
        Vector3 eye    = Position;
        Vector3 target = Position + Front;
        Vector3 up     = Up;

        return Matrix4.LookAt(eye, target, up);
    }
}
