#version 330 core
out vec4 FragColor;

in vec3 ourColor;
in vec2 TexCoord;

uniform bool hasWireframe;
uniform bool hasColor;
uniform bool hasTexture;

uniform sampler2D ourTexture;

void main()
{
    FragColor = vec4(1.0f);

    if(hasWireframe)
    {
        FragColor *= vec4(0.0f);
    }
    if(hasColor)
    {
        FragColor *= vec4(ourColor, 1.0f);
    }
    if(hasTexture)
    {
        FragColor *= texture(ourTexture, TexCoord);
    }
} 
