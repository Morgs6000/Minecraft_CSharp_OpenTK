using OpenTK.Mathematics;
using RubyDung.Common;

namespace RubyDung;

public class Camera
{
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

    private bool hasFly = true;

    public Camera()
    {
        MovementSpeed = hasFly ? flying : walking;
    }

    public void Update()
    {
        ProcessKeyboard();
    }

    private void ProcessKeyboard()
    {
        ProcessMovement();
        ProcessJump();
        ProcessSprinting();
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

    public Matrix4 LookAt()
    {
        Vector3 eye = Position;
        Vector3 target = Position + Front;
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
