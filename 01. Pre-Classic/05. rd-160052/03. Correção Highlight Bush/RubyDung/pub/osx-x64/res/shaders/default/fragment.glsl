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
        FragColor *= texture(ourTexture, TexCoord);
    }
    if(hasUniformColor)
    {
        FragColor *= vec4(uniformColor.x, uniformColor.y, uniformColor.z, uniformColor.w);
    }
}
