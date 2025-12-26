using System.Text.RegularExpressions;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace RubyDung;

public class Shader
{
    private int ID;

    public Shader(string shaderPath)
    {
        string shaderCode = File.ReadAllText(shaderPath);

        // Extrair vertex e fragment shader do arquivo combinado
        var shaders = ParseCombinedShader(RemoveComments(shaderCode));
        string vShaderCode = shaders.Vertex;
        string fShaderCode = shaders.Fragment;

        Build(vShaderCode, fShaderCode);
    }

    public Shader(string vertexPath, string fragmentPath)
    {
        string vShaderCode = RemoveComments(File.ReadAllText(vertexPath));
        string fShaderCode = RemoveComments(File.ReadAllText(fragmentPath));

        Build(vShaderCode, fShaderCode);
    }
    
    private void Build(string vShaderCode, string fShaderCode)
    {
        int vertex, fragment;
        int success;
        string infoLog;

        vertex = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertex, vShaderCode);
        GL.CompileShader(vertex);

        GL.GetShader(vertex, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            GL.GetShaderInfoLog(vertex, out infoLog);
            Debug.LogError("ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" + infoLog);
        }

        fragment = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragment, fShaderCode);
        GL.CompileShader(fragment);

        GL.GetShader(fragment, ShaderParameter.CompileStatus, out success);
        if (success == 0)
        {
            GL.GetShaderInfoLog(fragment, out infoLog);
            Debug.LogError("ERROR::SHADER::FRAGMENT::COMPILATION_FAILED\n" + infoLog);
        }

        ID = GL.CreateProgram();

        GL.AttachShader(ID, vertex);
        GL.AttachShader(ID, fragment);
        GL.LinkProgram(ID);

        GL.GetProgram(ID, GetProgramParameterName.LinkStatus, out success);
        if (success == 0)
        {
            GL.GetProgramInfoLog(ID, out infoLog);
            Debug.LogError("ERROR::SHADER::PROGRAM::LINKING_FAILED\n" + infoLog);
        }

        GL.DeleteShader(vertex);
        GL.DeleteShader(fragment);
    }

    public void Use()
    {
        GL.UseProgram(ID);
    }

    public int GetUniformLocation(string name)
    {
        return GL.GetUniformLocation(ID, name);
    }

    public void SetBool(string name, bool value)
    {
        int location = GetUniformLocation(name);
        GL.Uniform1(location, value ? 1 : 0);
    }

    public void SetInt(string name, int value)
    {
        int location = GetUniformLocation(name);
        GL.Uniform1(location, value);
    }

    public void SetFloat(string name, float value)
    {
        int location = GetUniformLocation(name);
        GL.Uniform1(location, value);
    }

    public void SetMatrix4(string name, Matrix4 matrix)
    {
        int location = GetUniformLocation(name);
        GL.UniformMatrix4(location, true, ref matrix);
    }

    // Método para remover comentários de código GLSL
    private string RemoveComments(string code)
    {
        if (string.IsNullOrEmpty(code))
            return code;

        // Remover comentários de linha única (// comentário)
        string pattern = @"//.*?$";
        string result = Regex.Replace(code, pattern, "", RegexOptions.Multiline);
        
        // Remover comentários multi-linha (/* comentário */)
        pattern = @"/\*.*?\*/";
        result = Regex.Replace(result, pattern, "", RegexOptions.Singleline);
        
        // Remover linhas vazias extras para limpeza
        result = Regex.Replace(result, @"^\s*$\n", "", RegexOptions.Multiline);
        
        return result.Trim();
    }

    // Método para analisar o shader combinado
    private (string Vertex, string Fragment) ParseCombinedShader(string shaderCode)
    {
        // Usar marcadores para separar os shaders
        string[] markers = { 
            "#vertex", 
            "#fragment", 
            "// vertex", 
            "// fragment",
            "#type vertex",
            "#type fragment"
        };

        string vertexShader = "";
        string fragmentShader = "";
        
        // Método 1: Usando marcador #vertex / #fragment
        if (shaderCode.Contains("#vertex") && shaderCode.Contains("#fragment"))
        {
            var parts = Regex.Split(shaderCode, @"(#vertex|#fragment)", RegexOptions.IgnoreCase);
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].ToLower().Contains("#vertex") && i + 1 < parts.Length)
                {
                    vertexShader = parts[i + 1].Trim();
                }
                else if (parts[i].ToLower().Contains("#fragment") && i + 1 < parts.Length)
                {
                    fragmentShader = parts[i + 1].Trim();
                }
            }
        }
        // Método 2: Usando marcador #type (estilo Hazel)
        else if (shaderCode.Contains("#type vertex") && shaderCode.Contains("#type fragment"))
        {
            var matches = Regex.Matches(shaderCode, @"#type\s+(\w+)\s*([^#]+)", RegexOptions.Singleline);
            foreach (Match match in matches)
            {
                string type = match.Groups[1].Value.Trim();
                string code = match.Groups[2].Value.Trim();
                
                if (type.ToLower() == "vertex")
                    vertexShader = code;
                else if (type.ToLower() == "fragment")
                    fragmentShader = code;
            }
        }
        // Método 3: Dividir por linha em branco duplo (simples)
        else
        {
            var parts = Regex.Split(shaderCode, @"\n\s*\n");
            if (parts.Length >= 2)
            {
                vertexShader = parts[0];
                fragmentShader = parts[1];
            }
            else
            {
                throw new InvalidOperationException("Formato de shader combinado inválido");
            }
        }

        return (vertexShader, fragmentShader);
    }
}
