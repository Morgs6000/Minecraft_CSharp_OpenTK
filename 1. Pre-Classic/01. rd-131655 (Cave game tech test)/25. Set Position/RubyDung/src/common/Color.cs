using OpenTK.Mathematics;

namespace RubyDung.common;

public class Color
{
    // Color.Rgba(0.2f, 0.3f, 0.3f)
    // Color.Rgba(0.2f, 0.3f, 0.3f, 1.0f)
    public static Color4 Rgba(float red, float green, float blue, float alpha = 1.0f)
    {
        float r = Math.Clamp(red, 0.0f, 1.0f);
        float g = Math.Clamp(green, 0.0f, 1.0f);
        float b = Math.Clamp(blue, 0.0f, 1.0f);
        float a = Math.Clamp(alpha, 0.0f, 1.0f);

        return new Color4(r, g, b, a);
    }

    // Color.Rgba(51, 76, 76)
    // Color.Rgba(51, 76, 76, 255)
    public static Color4 Rgba(int red, int green, int blue, int alpha = 255)
    {
        float r = Math.Clamp(red, 0, 255) / 255.0f;
        float g = Math.Clamp(green, 0, 255) / 255.0f;
        float b = Math.Clamp(blue, 0, 255) / 255.0f;
        float a = Math.Clamp(alpha, 0, 255) / 255.0f;

        return new Color4(r, g, b, a);
    }

    // Color.Hex("334C4C")
    // Color.Hex("#334C4C")
    // Color.Hex("334C4CFF")
    // Color.Hex("#334C4CFF")
    public static Color4 Hex(string hex)
    {
        return HexToRgba(hex);
    }

    // Color.Hex("334C4C", 1.0f)
    // Color.Hex("#334C4C", 1.0f)
    // Color.Hex("334C4CFF", 1.0f)
    // Color.Hex("#334C4CFF", 1.0f)
    public static Color4 Hex(string hex, float alpha)
    {
        float a = Math.Clamp(alpha, 0.0f, 1.0f);

        return HexToRgba(hex, a);
    }

    // Color.Hex("334C4C", 255)
    // Color.Hex("#334C4C", 255)
    // Color.Hex("334C4CFF", 255)
    // Color.Hex("#334C4CFF", 255)
    public static Color4 Hex(string hex, int alpha)
    {
        float a = Math.Clamp(alpha, 0, 255) / 255.0f;

        return HexToRgba(hex, a);
    }

    private static Color4 HexToRgba(string hex, float alpha = 1.0f)
    {
        // Remover o # se existir
        hex = hex.TrimStart('#');

        // Verificar formato do hexadecimal
        if (hex.Length != 6 && hex.Length != 8)
        {
            throw new ArgumentException("Formato hexadecimal inválido. Use 6 ou 8 caracteres (com ou sem alpha).");
        }

        // Converter componentes RGB
        float r = Convert.ToInt32(hex.Substring(0, 2), 16) / 255.0f;
        float g = Convert.ToInt32(hex.Substring(2, 2), 16) / 255.0f;
        float b = Convert.ToInt32(hex.Substring(4, 2), 16) / 255.0f;
        float a;

        // Converter componente Alpha (se existir)
        if (hex.Length == 8)
        {
            a = Convert.ToInt32(hex.Substring(6, 2), 16) / 255.0f;
        }
        else
        {
            // Usar o alpha fornecido como parâmetro
            a = Math.Clamp(alpha, 0.0f, 1.0f);
        }

        // Combinar alpha do hex com alpha do parâmetro (opcional)
        // Se você quiser que o parâmetro alpha multiplique/sobrescreva o alpha do hex:
        a *= alpha;

        return new Color4(r, g, b, a);
    }

    // Color.Hsv(180.0f, 0.32f, 0.29f)
    // Color.Hsv(180.0f, 0.32f, 0.29f, 1.0f)
    public static Color4 Hsv(float hue, float saturation, float value, float alpha = 1.0f)
    {
        float h = Math.Clamp(hue, 0.0f, 360.0f); // 0° a 360°
        float s = Math.Clamp(saturation, 0.0f, 1.0f);   // 0.0 a 1.0
        float v = Math.Clamp(value, 0.0f, 1.0f);   // 0.0 a 1.0
        float a = Math.Clamp(alpha, 0.0f, 1.0f);

        return HsvToRgba(h, s, v, a);
    }

    // Color.Hsv(180, 32, 29)
    // Color.Hsv(180, 32, 29, 255)
    public static Color4 Hsv(int hue, int saturation, int value, int alpha = 255)
    {
        float h = Math.Clamp(hue, 0, 360);          // 0° a 360°
        float s = Math.Clamp(saturation, 0, 100) / 100.0f; // 0 a 100
        float v = Math.Clamp(value, 0, 100) / 100.0f; // 0 a 100
        float a = Math.Clamp(alpha, 0, 255) / 255.0f;

        return HsvToRgba(h, s, v, a);
    }

    private static Color4 HsvToRgba(float hue, float saturation, float value, float alpha = 1.0f)
    {
        // Garantir que os valores estão dentro dos limites        
        float h = Math.Clamp(hue, 0.0f, 360.0f); // 0° a 360°
        float s = Math.Clamp(saturation, 0.0f, 1.0f);   // 0.0 a 1.0
        float v = Math.Clamp(value, 0.0f, 1.0f);   // 0.0 a 1.0
        float a = Math.Clamp(alpha, 0.0f, 1.0f);

        // Se saturação é 0, é uma cor acromática (escala de cinza)
        if (s <= 0.0f)
        {
            return new Color4(v, v, v, a);
        }

        // Normalizar hue para 0-6 (seis setores da roda de cores)
        float normalized = h / 60.0f;
        int sector = (int)Math.Floor(normalized);
        float fraction = normalized - sector;

        // Valores intermediários
        float p = v * (1.0f - s);
        float q = v * (1.0f - s * fraction);
        float t = v * (1.0f - s * (1.0f - fraction));

        switch (sector)
        {
            case 0: // Vermelho → Amarelo
                return new Color4(v, t, p, a);
            case 1: // Amarelo → Verde
                return new Color4(q, v, p, a);
            case 2: // Verde → Ciano
                return new Color4(p, v, t, a);
            case 3: // Ciano → Azul
                return new Color4(p, q, v, a);
            case 4: // Azul → Magenta
                return new Color4(t, p, v, a);
            case 5: // Magenta → Vermelho
                return new Color4(v, p, q, a);
            default:
                return new Color4(v, p, q, a);
        }
    }
}
