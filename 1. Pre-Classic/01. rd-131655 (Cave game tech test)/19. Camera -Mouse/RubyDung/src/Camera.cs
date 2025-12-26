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

    private bool fistMouse = true;
    private Vector2 last;

    private float pitch;        // rotX // inclinação
    private float yaw = -90.0f; // rotY // guinada
    // private float roll;         // rotZ // rolamento
    
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
        
    }

    private void ProcessKeyboard()
    {
        Movement();
        Sprinting();
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
        if(Input.GetKey(KeyCode.W))
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

    public Matrix4 LookAt()
    {
        Vector3 eye    = Position;
        Vector3 target = Position + Front;
        Vector3 up     = Up;

        return Matrix4.LookAt(eye, target, up);
    }
}
