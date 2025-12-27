using OpenTK.Mathematics;
using RubyDung.common;
using RubyDung.level;
using RubyDung.phys;

namespace RubyDung;

public class Camera
{
    private Level level;
    private AABB cameraBox = null!;
    private GameMode gameMode;

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

    private bool fistMouse = true;
    private Vector2 last;

    private float pitch;        // rotX // inclinação
    private float yaw = -90.0f; // rotY // guinada
    // private float roll;         // rotZ // rolamento

    private float playerWidht = 0.6f;
    
    private float playerHeight = 1.8f;
    private float eyeHeight    = 1.62f;
    
    // Valores quando agachado
    // private float SNEAKING_PLAYER_HEIGHT = 1.5f;
    // private float SNEAKING_EYE_HEIGHT    = 1.32f;
    
    // Variável para armazenar a altura anterior (para ajuste suave)
    // private float previousPlayerHeight;

    private bool hasFly;
    private bool hasCollision;
    private bool hasGravity;
    // private bool hasSneaking;

    private Vector3 velocity;
    private bool onGround;

    // Adicione essas variáveis para controle do delay
    private float initializationTimer = 0.0f;
    private const float INITIALIZATION_DELAY = 1.0f; // 0.5 segundos de delay
    private bool isInitialized = false;

    public Camera(Level level)
    {
        this.level = level;

        ResetPos();

        MovementSpeed = hasFly ? flying : walking;

        // previousPlayerHeight = playerHeight;

        gameMode = GameMode.Creative;
        ProcessGameMode();
    }

    public void Update(bool hasPause)
    {
        // Delay de inicialização antes de processar qualquer coisa
        if (!isInitialized)
        {
            initializationTimer += Time.deltaTime;
            
            if (initializationTimer >= INITIALIZATION_DELAY)
            {
                isInitialized = true;
                // Não chama ResetPos() novamente para não alterar sua lógica
            }
            else
            {
                // Durante o delay, não processa nenhum input ou física
                return;
            }
        }

        if (hasPause)
        {
            fistMouse = true;
        }
        else
        {
            ProcessKeyboard();
            ProcessMouse();

            if (hasCollision)
            {
                ProcessCollision();
            }
            if (!hasFly && hasGravity)
            {
                ProcessGravity();
            }
        }
    }

    private void ResetPos()
    {
        Random random = new Random();

        float x = (float)(random.NextDouble() * level.width);
        float y = (float)(level.height + 10);
        float z = (float)(random.NextDouble() * level.depth);

        SetPos(x, y, z);

        onGround = false;
    }
    
    private void SetPos(float x, float y, float z)
    {
        Position = new Vector3(x, y, z);
    }

    private void ProcessKeyboard()
    {
        ProcessMovement();
        ProcessSprinting();

        if (hasFly)
        {
            ProcessFly();
        }
        else
        {
            ProcessJump();
            ProcessSneaking();
        }

        // Debug.Log(MovementSpeed);

        if (Input.GetKey(KeyCode.R))
        {
            ResetPos();
        }
        if (Input.GetKeyDouble(KeyCode.Space) && gameMode == GameMode.Creative)
        {
            ToggleFly();
        }
    }
    
    private void ToggleFly()
    {
        hasFly = !hasFly;
        MovementSpeed = hasFly ? flying : walking;

        Debug.Log($"hasFly: {hasFly}");
    }

    private void ProcessMouse()
    {
        ProcessMouseMovement();
        ProcessMouseScroll();
    }

    private void ProcessMovement()
    {
        float speed = MovementSpeed * Time.deltaTime;

        float x = 0.0f;
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

        Position += x * speed * Vector3.Normalize(Vector3.Cross(Front, Up));
        Position += z * speed * Vector3.Normalize(new Vector3(Front.X, 0.0f, Front.Z));
    }

    private void ProcessFly()
    {
        float speed = MovementSpeed * Time.deltaTime;

        float y = 0.0f;

        if (Input.GetKey(KeyCode.Space))
        {
            y++;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            y--;
        }

        Position += y * speed * Up;
    }

    private void ProcessSprinting()
    {
        if (Input.GetKeyDouble(KeyCode.W))
        {
            MovementSpeed = hasFly ? sprintFlying : sprinting;
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                MovementSpeed = hasFly ? sprintFlying : sprinting;
            }
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            MovementSpeed = hasFly ? flying : walking;
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

    private void ProcessMouseScroll()
    {
        // Implementação do scroll do mouse (se necessário)
    }

    private void ProcessCollision()
    {
        UpdateCameraBox();

        // onGround = false;
        
        List<AABB> cubes = level.GetCubes(cameraBox);

        foreach (AABB cube in cubes)
        {
            // AABB cube = new AABB(0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f);

            if (cameraBox.Intersects(cube))
            {
                // Debug.Log("Colisão detectada com bloco!");

                Vector3 overlap = cameraBox.CalculateOverlap(cube);

                if (overlap.X < overlap.Y && overlap.X < overlap.Z)
                {
                    Position.X = cameraBox.ClipXCollide(cube, Position.X);
                    UpdateCameraBox();
                }
                if (overlap.Y < overlap.X && overlap.Y < overlap.Z)
                {
                    Position.Y = cameraBox.ClipYCollide(cube, Position.Y);
                    UpdateCameraBox();

                    onGround = true;
                    // hasFly = false;
                }
                if (overlap.Z < overlap.X && overlap.Z < overlap.Y)
                {
                    Position.Z = cameraBox.ClipZCollide(cube, Position.Z);
                    UpdateCameraBox();
                }
            }
        }

        Debug.Log($"Position: {Position.X:f0}, {Position.Y:f2}, {Position.Z:f0}; onGround: {onGround}", true);
    }

    private void UpdateCameraBox()
    {
        /*
        float x0 = Position.X - (playerWidht  / 2.0f);
        float y0 = Position.Y - (playerHeight / 2.0f);
        float z0 = Position.Z - (playerWidht  / 2.0f);

        float x1 = Position.X + (playerWidht  / 2.0f);
        float y1 = Position.Y + (playerHeight / 2.0f);
        float z1 = Position.Z + (playerWidht / 2.0f);
        //*/
        
        //*
        float x0 = Position.X - (playerWidht  / 2.0f);
        float y0 = Position.Y - eyeHeight;
        float z0 = Position.Z - (playerWidht  / 2.0f);

        float x1 = Position.X + (playerWidht  / 2.0f);
        float y1 = (Position.Y - eyeHeight) + playerHeight;
        float z1 = Position.Z + (playerWidht  / 2.0f);
        //*/

        /*
        float x0 = Position.X - (playerWidht / 2.0f);
        float y0 = Position.Y - eyeHeight;
        float z0 = Position.Z - (playerWidht / 2.0f);

        float x1 = Position.X + (playerWidht / 2.0f);
        float y1 = Position.Y + (playerHeight - eyeHeight);
        float z1 = Position.Z + (playerWidht / 2.0f);
        //*/

        /*
        float x0 = Position.X;
        float y0 = Position.Y;
        float z0 = Position.Z;

        float x1 = Position.X + playerWidht;
        float y1 = Position.Y + playerHeight;
        float z1 = Position.Z + playerWidht;
        //*/

        cameraBox = new AABB(x0, y0, z0, x1, y1, z1);
    }
    
    private void ProcessGravity()
    {
        if(onGround && velocity.Y < 0)
        {
            velocity.Y = -2.0f;
        }

        velocity.Y -= falling * Time.deltaTime;
        Position += velocity * Time.deltaTime;
    }

    private void ProcessJump()
    {
        if (Input.GetKey(KeyCode.Space) && onGround)
        {
            onGround = false;

            velocity.Y = MathF.Sqrt(jumping * 2.0f * falling);
        }        
    }

    private void ProcessSneaking()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // hasSneaking = true;

            MovementSpeed = sneaking;

            // previousPlayerHeight = playerHeight;
            // playerHeight = SNEAKING_PLAYER_HEIGHT;
            // eyeHeight = SNEAKING_EYE_HEIGHT;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            // hasSneaking = false;

            MovementSpeed = walking;

            // playerHeight = 1.8f;
            // eyeHeight = 1.62f;
        }
    }

    private void ProcessGameMode()
    {
        switch (gameMode)
        {
            case GameMode.Survival:
                hasFly = false;
                hasCollision = true;
                hasGravity = true;
                break;
            case GameMode.Creative:
                hasFly = false;
                hasCollision = true;
                hasGravity = true;
                break;
            case GameMode.Adventure:
                hasFly = false;
                hasCollision = true;
                hasGravity = true;
                break;
            case GameMode.Spectator:
                hasFly = true;
                hasCollision = false;
                hasGravity = false;
                break;
        }
    }

    public Matrix4 LookAt()
    {
        //*
        Vector3 eye = Position;
        Vector3 target = Position + Front;
        Vector3 up = Up;
        //*/

        /*
        Vector3 eyeOffset = new Vector3(
            Position.X + (playerWidht / 2.0f),  // Centro na largura
            Position.Y + eyeHeight,             // Altura dos olhos
            Position.Z + (playerWidht / 2.0f)   // Centro na profundidade
        );

        Vector3 eye = eyeOffset;
        Vector3 target = eyeOffset + Front;
        Vector3 up = Up;
        //*/

        return Matrix4.LookAt(eye, target, up);
    }

    public Matrix4 CreatePerspectiveFieldOfView()
    {
        float fovy = MathHelper.DegreesToRadians(70.0f);
        float aspect = (float)Screen.widht / (float)Screen.height;
        float depthNear = 0.05f;
        float depthFar = 1000.0f;

        return Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, depthNear, depthFar);
    }
}
