using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;

namespace Minecraft;

public class Font
{
    private GameWindow gameWindow;
    private Texture texture;
    private Mesh mesh;

    private int[] charWidths = new int[256];

    private float deltaTime = 0.0f;
    private float lastFrame = 0.0f;

    private float timeAccumulator = 0.0f;
    private int frames = 0;
    // private double nextUpdate = 0.0d;
    private string fpsString = "";

    public Font(GameWindow gameWindow)
    {
        this.gameWindow = gameWindow;

        string texturePath = "res/textures/default.gif";
        texture = new Texture(texturePath);

        mesh = new Mesh();

        byte[] fileData = File.ReadAllBytes(texturePath);
        ImageResult image = ImageResult.FromMemory(fileData, ColorComponents.RedGreenBlueAlpha);

        int w = image.Width;
        int h = image.Height;
        byte[] rawPixels = image.Data;

        for (int i = 0; i < 128; i++)
        {
            // charWidths[i] = 8;

            int xt = i % 16;
            int yt = i / 16;
            int x = 0;
            bool emptyColumn = false;

            for (x = 0; x < 8 && !emptyColumn; ++x)
            {
                int xPixel = xt * 8 + x;
                emptyColumn = true;

                for (int y = 0; y < 8 && emptyColumn; ++y)
                {
                    int yPixel = (yt * 8 + y) * w;

                    int pixelIndex = (xPixel + yPixel) * 4;

                    int pixelValue = rawPixels[pixelIndex];

                    if (pixelValue > 128)
                    {
                        emptyColumn = false;
                    }
                }
            }

            if (i == 32)
            {
                x = 4;
            }

            this.charWidths[i] = x;
        }

        /*
        charWidths[' '] = 5;
        charWidths['!'] = 1;
        charWidths['"'] = 4;
        charWidths['#'] = 5;
        charWidths['$'] = 5;
        charWidths['%'] = 6;
        charWidths['&'] = 6;
        // charWidths['''] = 2;
        charWidths['('] = 4;
        charWidths[')'] = 4;
        charWidths['*'] = 7;
        charWidths['+'] = 5;
        charWidths[','] = 1;
        charWidths['-'] = 5;
        charWidths['.'] = 1;
        charWidths['/'] = 5;

        charWidths['0'] = 5;
        charWidths['1'] = 5;
        charWidths['2'] = 5;
        charWidths['3'] = 5;
        charWidths['4'] = 5;
        charWidths['5'] = 5;
        charWidths['6'] = 5;
        charWidths['7'] = 5;
        charWidths['8'] = 5;
        charWidths['9'] = 5;
        charWidths[':'] = 1;
        charWidths[';'] = 1;
        charWidths['<'] = 4;
        charWidths['='] = 5;
        charWidths['>'] = 4;
        charWidths['?'] = 5;

        charWidths['@'] = 6;
        charWidths['A'] = 5;
        charWidths['B'] = 5;
        charWidths['C'] = 5;
        charWidths['D'] = 5;
        charWidths['E'] = 5;
        charWidths['F'] = 5;
        charWidths['G'] = 5;
        charWidths['H'] = 5;
        charWidths['I'] = 1;
        charWidths['J'] = 5;
        charWidths['K'] = 5;
        charWidths['L'] = 5;
        charWidths['M'] = 5;
        charWidths['N'] = 5;
        charWidths['O'] = 5;

        charWidths['P'] = 5;
        charWidths['Q'] = 5;
        charWidths['R'] = 5;
        charWidths['S'] = 5;
        charWidths['T'] = 5;
        charWidths['U'] = 5;
        charWidths['V'] = 5;
        charWidths['W'] = 5;
        charWidths['X'] = 5;
        charWidths['Y'] = 5;
        charWidths['Z'] = 5;
        charWidths['['] = 3;
        // charWidths['\'] = 5;
        charWidths[']'] = 3;
        charWidths['^'] = 5;
        charWidths['_'] = 5;

        // charWidths[''] = 2;
        charWidths['a'] = 5;
        charWidths['b'] = 5;
        charWidths['c'] = 5;
        charWidths['d'] = 5;
        charWidths['e'] = 5;
        charWidths['f'] = 4;
        charWidths['g'] = 5;
        charWidths['h'] = 5;
        charWidths['i'] = 1;
        charWidths['j'] = 5;
        charWidths['k'] = 4;
        charWidths['l'] = 2;
        charWidths['m'] = 5;
        charWidths['n'] = 5;
        charWidths['o'] = 5;

        charWidths['p'] = 5;
        charWidths['q'] = 5;
        charWidths['r'] = 5;
        charWidths['s'] = 5;
        charWidths['t'] = 3;
        charWidths['u'] = 5;
        charWidths['v'] = 5;
        charWidths['w'] = 5;
        charWidths['x'] = 5;
        charWidths['y'] = 5;
        charWidths['z'] = 5;
        charWidths['{'] = 5;
        charWidths['|'] = 4;
        charWidths['}'] = 5;
        charWidths['~'] = 6;
        // charWidths[''] = 5;
        */
    }

    public void Load()
    {
        // Console.WriteLine(ConferirCor(16777215));
        // Console.WriteLine(ConferirCor(16579836));
    }
    
    private string ConferirCor(int c)
    {
        float r = (float)(c >> 16 & 255) / 255.0f;
        float g = (float)(c >> 8 & 255) / 255.0f;
        float b = (float)(c & 255) / 255.0f;

        return $"{r}, {g}, {b}";
        // return $"{r * 255}, {g * 255}, {b * 255}";
    }

    public void Update()
    {
        ProcessTime();

        /*
        double frames = 1.0d / deltaTime;
        fpsString = $"{frames} fps, {Chunk.updates} chunk updates";
        */
        /*
        frames++;
        double currentTime = GLFW.GetTime();

        if(currentTime >= nextUpdate)
        {
            fpsString = $"{frames} fps, {Chunk.updates} chunk updates";
            frames = 0;
            nextUpdate = currentTime + 1.0d;
        }
        */
        frames++;
        timeAccumulator += deltaTime;

        if(timeAccumulator >= 1.0f)
        {
            fpsString = $"{frames} fps, {Chunk.updates} chunk updates";

            frames = 0;
            timeAccumulator %= 1.0f;
        }        

        int screenHeight = gameWindow.ClientSize.Y * 240 / gameWindow.ClientSize.Y;

        mesh.Begin();

        DrawShadow("0.0.10a", 2, (screenHeight - 8) - 2, 16777215);
        DrawShadow(fpsString, 2, (screenHeight - 8) - 12, 16777215);

        mesh.End();
    }

    public void Render(Shader shader)
    {
        GL.Disable(EnableCap.DepthTest);

        Matrix4 model = Matrix4.Identity;
        shader.SetMatrix4("model", model);

        Matrix4 view = Matrix4.Identity;
        shader.SetMatrix4("view", view);

        Matrix4 projection = Matrix4.Identity;
        projection *= CreateOrthographicOffCenter();
        shader.SetMatrix4("projection", projection);

        shader.SetBool("hasUniformColor", true);
        shader.SetVector4("uniformColor", 1.0f, 1.0f, 1.0f, 1.0f);

        texture.Bind();

        mesh.Render(shader);

        GL.Enable(EnableCap.DepthTest);
    }

    private void ProcessTime()
    {
        float currentFrame = (float)GLFW.GetTime();
        deltaTime = currentFrame - lastFrame;
        lastFrame = currentFrame;
    }

    public Matrix4 CreateOrthographicOffCenter()
    {
        int screenWidth = gameWindow.ClientSize.X * 240 / gameWindow.ClientSize.Y;
        int screenHeight = gameWindow.ClientSize.Y * 240 / gameWindow.ClientSize.Y;

        float left = 0.0f;
        float right = (float)screenWidth;
        float bottom = 0.0f;
        float top = (float)screenHeight;
        float depthNear = 0.0f;
        float depthFar = 100.0f;

        return Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, depthNear, depthFar);
    }

    public void DrawShadow(string str, int x, int y, int color)
    {
        Draw(str, x + 1, y - 1, color, true);
        Draw(str, x, y, color);
    }

    public void Draw(string str, int x, int y, int color)
    {
        Draw(str, x, y, color, false);
    }
    
    public void Draw(string str, int x, int y, int color, bool darken)
    {
        char[] chars = str.ToCharArray();

        if (darken)
        {
            color = (color & 16579836) >> 2;
        }

        // mesh.Begin();

        mesh.Color(color);

        int xo = 0;

        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] == '&')
            {
                int cc = "0123456789abcdef".IndexOf(chars[i + 1]);
                int br = (cc & 8) * 8;

                int b = (cc & 1) * 191 + br;
                int g = ((cc & 2) >> 1) * 191 + br;
                int r = ((cc & 4) >> 2) * 191 + br;

                color = r << 16 | g << 8 | b;

                i += 2;

                if (darken)
                {
                    color = (color & 16579836) >> 2;
                }

                mesh.Color(color);
            }

            int ix = chars[i] % 16 * 8;
            int iy = chars[i] / 16 * 8;

            float x0 = (float)(x + xo);
            float y0 = (float)y;

            float x1 = (float)(x + xo + 8);
            float y1 = (float)(y + 8);

            float u0 = (float)ix / 128.0f;
            float v0 = (float)iy / 128.0F;

            float u1 = (float)(ix + 8) / 128.0F;
            float v1 = (float)(iy + 8) / 128.0F;

            mesh.VertexUV(x0, y0, 0.0F, u0, v1);
            mesh.VertexUV(x1, y0, 0.0F, u1, v1);
            mesh.VertexUV(x1, y1, 0.0F, u1, v0);
            mesh.VertexUV(x0, y1, 0.0F, u0, v0);

            // xo += charWidths[chars[i]];
            xo += charWidths[chars[i]];
        }

        // mesh.End();
    }
}
