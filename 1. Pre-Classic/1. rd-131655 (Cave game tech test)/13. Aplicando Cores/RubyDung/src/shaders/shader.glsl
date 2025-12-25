#vertex
#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;

out vec3 ourColor;

void main()
{
    gl_Position = vec4(aPosition, 1.0f);

    ourColor = aColor;
}

#fragment
#version 330 core
out vec4 FragColor;

in vec3 ourColor;

uniform bool hasWireframe;
uniform bool hasColor;

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
}  
