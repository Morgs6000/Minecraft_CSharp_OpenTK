#version 330 core
out vec4 FragColor;

in vec3 ourColor;
in vec2 TexCoord;

uniform bool hasColor;
uniform bool hasTexture;
uniform bool hasUniformColor;

uniform vec4 uniformColor;

uniform sampler2D ourTexture;

void main()
{
    FragColor = vec4(1.0f, 1.0f, 1.0f, 1.0f);

    if(hasColor)
    {
        FragColor *= vec4(ourColor.x, ourColor.y, ourColor.z, 1.0f);
    }
    if(hasTexture)
    {
        vec4 texColor = texture(ourTexture, TexCoord);
        if(texColor.a < 0.1f)
        {
            discard;
        }

        FragColor *= texColor;
    }
    if(hasUniformColor)
    {
        FragColor *= vec4(uniformColor.x, uniformColor.y, uniformColor.z, uniformColor.w);
    }
}
