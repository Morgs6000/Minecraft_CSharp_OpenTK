using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RubyDung;

public class Camera
{
    private Level level;
    private AABB cameraBox = null!;

    private Vector3 Position;
    private Vector3 Front;
    private Vector3 Up;

    public Vector3 position
    {
        get
        {
            return Position;
        }
    }

    public Vector3 front
    {
        get
        {
            return Front;
        }
    }

    private float Pitch;        // rotX // inclinação
    private float Yaw = -90.0f; // rotY // guinada
    // private float Roll;         // rotZ // rotação

    private float MovementSpeed = 0.0f;
    private float MouseSensitivity;

    private float deltaTime = 0.0f;
    private float lastFrame = 0.0f;

    private bool fistMouse = true;
    private Vector2 lastPos;

    private float playerWidht = 0.6f;    
    private float playerHeight = 1.8f;
    private float eyeHeight = 1.62f;

    private Vector3 velocity;
    private bool onGround;
    
    private float walking = 4.317f;
    private float falling = 77.71f;
    private float jumping = 1.2522f;

    private float cameraStartDelay = 5.0f; // 3 segundos de delay
    private float cameraDelayTimer = 0.0f;
    private bool cameraEnabled = false;

    public Camera(Level level)
    {
        this.level = level;

        Position = new Vector3(0.0f, 0.0f, 3.0f);
        Front = new Vector3(0.0f, 0.0f, -1.0f);
        Up = new Vector3(0.0f, 1.0f, 0.0f);

        MovementSpeed = walking;
        MouseSensitivity = 0.1f;

        ResetPos();

        // Inicia com a câmera desabilitada
        cameraEnabled = false;
        cameraDelayTimer = cameraStartDelay;
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

    public void Update(GameWindow gameWindow)
    {        
        KeyboardState keyboardState = gameWindow.KeyboardState;
        MouseState mouseState = gameWindow.MouseState;

        // Atualiza o timer de delay
        if (!cameraEnabled)
        {
            cameraDelayTimer -= deltaTime;
            if (cameraDelayTimer <= 0.0f)
            {
                cameraEnabled = true;
                cameraDelayTimer = 0.0f;
            }
            
            // Não processa inputs enquanto a câmera estiver desabilitada
            ProcessTime();
            return;
        }

        ProcessTime();
        ProcessKeyboard(keyboardState);
        
        ProcessMouseMovement(mouseState);
        gameWindow.CursorState = CursorState.Grabbed;

        ProcessCollision();
        ProcessGravity();

        if (keyboardState.IsKeyDown(Keys.R))
        {
            ResetPos();
        }
    }

    private void ProcessTime()
    {
        float currentFrame = (float)GLFW.GetTime();
        deltaTime = currentFrame - lastFrame;
        lastFrame = currentFrame;
    }

    private void ProcessKeyboard(KeyboardState keyboardState)
    {
        float speed = MovementSpeed * deltaTime;

        if (keyboardState.IsKeyDown(Keys.W))
        {
            Position += speed * Vector3.Normalize(new Vector3(Front.X, 0.0f, Front.Z));
        }
        if (keyboardState.IsKeyDown(Keys.S))
        {
            Position -= speed * Vector3.Normalize(new Vector3(Front.X, 0.0f, Front.Z));
        }
        if (keyboardState.IsKeyDown(Keys.A))
        {
            Position -= speed * Vector3.Normalize(Vector3.Cross(Front, Up));
        }
        if (keyboardState.IsKeyDown(Keys.D))
        {
            Position += speed * Vector3.Normalize(Vector3.Cross(Front, Up));
        }
        if (keyboardState.IsKeyDown(Keys.Space) && onGround)
        {
            onGround = false;

            velocity.Y = MathF.Sqrt(jumping * 2.0f * falling);
        }
    }

    private void ProcessMouseMovement(MouseState mouseState)
    {
        if (fistMouse)
        {
            lastPos.X = mouseState.Position.X;
            lastPos.Y = mouseState.Position.Y;

            fistMouse = false;
        }

        float xoffset = mouseState.Position.X - lastPos.X;
        float yoffset = lastPos.Y - mouseState.Position.Y;

        lastPos.X = mouseState.Position.X;
        lastPos.Y = mouseState.Position.Y;

        xoffset *= MouseSensitivity;
        yoffset *= MouseSensitivity;

        Yaw += xoffset;
        Pitch += yoffset;

        Pitch = MathHelper.Clamp(Pitch, -89.0f, 89.0f);

        UpdateCameraVectors();
    }

    private void UpdateCameraVectors()
    {
        Vector3 front;
        front.X = (float)(Math.Cos(MathHelper.DegreesToRadians(Pitch)) * Math.Cos(MathHelper.DegreesToRadians(Yaw)));
        front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(Pitch));
        front.Z = (float)(Math.Cos(MathHelper.DegreesToRadians(Pitch)) * Math.Sin(MathHelper.DegreesToRadians(Yaw)));

        Front = Vector3.Normalize(front);
    }

    private void ProcessCollision()
    {
        UpdateCameraBox();
        
        List<AABB> cubes = level.GetCubes(cameraBox);

        foreach (AABB cube in cubes)
        {
            if (cameraBox.Intersects(cube))
            {
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
                }
                if (overlap.Z < overlap.X && overlap.Z < overlap.Y)
                {
                    Position.Z = cameraBox.ClipZCollide(cube, Position.Z);

                    UpdateCameraBox();
                }
            }
        }
    }

    private void UpdateCameraBox()
    {
        float x = Position.X;
        float y = Position.Y - eyeHeight;
        float z = Position.Z;

        float w = (playerWidht / 2.0f);
        float h = (playerHeight / 2.0f);

        float x0 = x - w;
        float y0 = y - h;
        float z0 = z - w;

        float x1 = x + w;
        float y1 = y + h;
        float z1 = z + w;

        cameraBox = new AABB(x0, y0, z0, x1, y1, z1);
    }
    
    private void ProcessGravity()
    {
        if (onGround && velocity.Y < 0)
        {
            velocity.Y = -2.0f;
        }
        
        velocity.Y -= falling * deltaTime;
        Position += velocity * deltaTime;
    }

    public Matrix4 LookAt()
    {
        Vector3 eye = Position;
        Vector3 target = Position + Front;
        Vector3 up = Up;

        return Matrix4.LookAt(eye, target, up);
    }
    
    public Matrix4 CreatePerspectiveFieldOfView(GameWindow gameWindow)
    {
        float fovy = MathHelper.DegreesToRadians(70.0f);
        float aspect = (float)gameWindow.ClientSize.X / (float)gameWindow.ClientSize.Y;
        float depthNear = 0.05f;
        float depthFar = 1000.0f;

        return Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, depthNear, depthFar);
    }
}
