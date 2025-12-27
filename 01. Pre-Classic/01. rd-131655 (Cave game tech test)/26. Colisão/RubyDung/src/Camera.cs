using OpenTK.Mathematics;
using RubyDung.common;
using RubyDung.level;
using RubyDung.phys;

namespace RubyDung;

public class Camera
{
    private Level level;
    private AABB cameraBox = null!;

    private Vector3 Position = new Vector3(0.0f, 0.0f, 3.0f);
    private Vector3 Front    = new Vector3(0.0f, 0.0f, -1.0f);
    private Vector3 Up       = new Vector3(0.0f, 1.0f, 0.0f);
    
    private float MovementSpeed;
    
    private float walking      = 4.317f;
    private float sprinting    = 5.612f;
    // private float sneaking     = 1.295f;
    private float flying       = 10.79f;
    private float sprintFlying = 21.58f;

    // private float falling      = 77.71f;
    // private float jumping      = 1.2522f;

    private float playerWidht  = 0.6f;
    private float playerHeight = 1.8f;
    private float eyeHeight    = 1.62f;

    private bool hasFly = true;
    private bool hasCollision = true;

    private bool fistMouse = true;
    private Vector2 last;

    private float pitch;        // rotX // inclinação
    private float yaw = -90.0f; // rotY // guinada
    // private float roll;         // rotZ // rolamento

    public Camera(Level level)
    {
        this.level = level;

        ResetPos();

        MovementSpeed = hasFly ? flying : walking;
    }

    public void Update(bool hasPause)
    {
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
                CheckCollisions();
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
    }
    
    private void SetPos(float x, float y, float z)
    {
        Position = new Vector3(x, y, z);
    }

    private void ProcessKeyboard()
    {
        ProcessMovement();
        ProcessJump();
        ProcessSprinting();

        if (Input.GetKey(KeyCode.R))
        {
            ResetPos();
        }
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

    private void ProcessJump()
    {
        float speed = MovementSpeed * Time.deltaTime;

        float y = 0.0f;

        if (!hasFly)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                // Pulo
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

        if (Input.GetKeyDouble(KeyCode.Space))
        {
            hasFly = !hasFly;
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

    }

    private void CheckCollisions()
    {
        UpdateCameraBox();

        // Debug.Log("Teste");

        
        List<AABB> cubes = level.GetCubes(cameraBox);

        foreach (AABB cube in cubes)
        {
            // Debug.Log("Teste");

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
        float x0 = Position.X;
        float y0 = Position.Y;
        float z0 = Position.Z;

        float x1 = Position.X + playerWidht;
        float y1 = Position.Y + playerHeight;
        float z1 = Position.Z + playerWidht;

        cameraBox = new AABB(x0, y0, z0, x1, y1, z1);
    }

    public Matrix4 LookAt()
    {
        Vector3 eyeOffset = new Vector3(
            Position.X + (playerWidht / 2.0f),  // Centro na largura
            Position.Y + eyeHeight,             // Altura dos olhos
            Position.Z + (playerWidht / 2.0f)   // Centro na profundidade
        );

        Vector3 eye = eyeOffset;
        Vector3 target = eyeOffset + Front;
        Vector3 up = Up;

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
