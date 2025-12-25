#vertex
#version 330 core
layout (location = 0) in vec3 aPosition;

void main()
{
    gl_Position = vec4(aPosition, 1.0f);
}

#fragment
#version 330 core
out vec4 FragColor;

uniform bool hasWireframe;

void main()
{
    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);

    if(hasWireframe)
    {
        FragColor *= vec4(0.0f);
    }
}  
