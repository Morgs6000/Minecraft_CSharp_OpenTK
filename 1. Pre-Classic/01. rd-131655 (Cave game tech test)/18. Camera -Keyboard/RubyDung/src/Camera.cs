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
        Sprinting();
    }

    private void ProcessKeyboard()
    {
        // float speed = flying * Time.deltaTime;
        float speed = MovementSpeed * Time.deltaTime;

        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        if (Input.GetKey(KeyCode.W))
        {
            // Position += speed * Vector3.Normalize(new Vector3(Front.X, 0.0f, Front.Z));
            z++;
        }
        if (Input.GetKey(KeyCode.S))
        {
            // Position -= speed * Vector3.Normalize(new Vector3(Front.X, 0.0f, Front.Z));
            z--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            // Position += speed * Vector3.Normalize(Vector3.Cross(Front, Up));
            x++;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // Position -= speed * Vector3.Normalize(Vector3.Cross(Front, Up));
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
                // Position += speed * Up;
                y++;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // Position -= speed * Up;
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
