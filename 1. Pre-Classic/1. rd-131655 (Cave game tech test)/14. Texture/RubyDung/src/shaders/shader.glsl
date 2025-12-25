#vertex
#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;
layout (location = 2) in vec2 aTexCoord;

out vec3 ourColor;
out vec2 TexCoord;

void main()
{
    gl_Position = vec4(aPosition, 1.0f);

    ourColor = aColor;
    
    TexCoord = aTexCoord;
}

#fragment
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
