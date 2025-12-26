using OpenTK.Graphics.OpenGL4;
using RubyDung.Common;
using StbImageSharp;

namespace RubyDung;

public class Texture
{
    private uint texture;

    public Texture(string path)
    {
        GL.GenTextures(1, out texture);
        GL.BindTexture(TextureTarget.Texture2D, texture);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.MirroredRepeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.MirroredRepeat);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);

        if (image.Data != null)
        {
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        else
        {
            Debug.LogError("Falha ao carregar a textura");
        }
    }

    public void Bind()
    {
        GL.BindTexture(TextureTarget.Texture2D, texture);
    }
}
