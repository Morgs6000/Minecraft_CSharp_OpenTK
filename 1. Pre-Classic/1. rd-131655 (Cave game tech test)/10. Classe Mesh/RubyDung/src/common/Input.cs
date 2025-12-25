using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public class Input
{
    private static KeyboardState keyboardState = null!;
    private static MouseState mouseState = null!;

    // Dicionário para armazenar tempos do último clique de cada tecla(para double click)
    private static Dictionary<KeyCode, float> lastClickTimes = new Dictionary<KeyCode, float>();

    // Tempo máximo entre cliques para ser cconsiderado double click (200ms)
    private static float DOUBLE_CLICK_TIME = 0.2f;

    /// <summary>
    /// Alguma tecla ou botão do mouse está pressionado no momento? (Somente leitura)
    /// </summary>
    public static bool anyKey
    {
        get
        {
            if (keyboardState.IsAnyKeyDown)
            {
                return true;
            }
            if (mouseState.IsAnyButtonDown)
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Retorna verdadeiro no primeiro frame em que o usuário pressionar qualquer tecla ou botão do mouse. (Somente leitura)
    /// </summary>
    public static bool anyKeyDown
    {
        get
        {
            foreach (KeyCode key in Enum.GetValues(typeof(Keys)))
            {
                if (key != KeyCode.None)
                {
                    if (GetKeyDown(key))
                    {
                        return true;
                    }
                }
            }
            foreach (KeyCode button in Enum.GetValues(typeof(MouseButton)))
            {
                if (GetMouseButtonDown(button))
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Retorna verdadeiro no primeiro frame em que o usuário solta qualquer tecla ou botão do mouse. (Somente leitura)
    /// </summary>
    public static bool anyKeyUp
    {
        get
        {
            foreach (KeyCode key in Enum.GetValues(typeof(Keys)))
            {
                if (key != KeyCode.None)
                {
                    if (GetKeyUp(key))
                    {
                        return true;
                    }
                }
            }
            foreach (KeyCode button in Enum.GetValues(typeof(MouseButton)))
            {
                if (GetMouseButtonUp(button))
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Posição atual do cursor em coordenadas de pixel. (Somente leitura).
    /// </summary>
    public static Vector2 mousePosition
    {
        get
        {
            return mouseState.Position;
        }
    }

    /// <summary>
    /// Delta de rolagem atual do mouse. (Somente leitura)
    /// </summary>
    public static Vector2 mouseScrollDelta
    {
        get
        {
            return mouseState.ScrollDelta;
        }
    }

    /// <summary>
    /// Retorna a primeira tecla ou botão do mouse pressionado no moemnto. (Somente leitura)
    /// </summary>
    public static string pressedKey
    {
        get
        {
            foreach (KeyCode key in Enum.GetValues(typeof(Keys)))
            {
                if (key != KeyCode.None)
                {
                    if (GetKey(key) || GetKeyDown(key) || GetKeyUp(key))
                    {
                        return key.ToString();
                    }
                }
            }
            foreach (KeyCode button in Enum.GetValues(typeof(MouseButton)))
            {
                if (GetMouseButton(button) || GetMouseButtonDown(button) || GetMouseButtonUp(button))
                {
                    return button.ToString();
                }
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// Atualiza os estados de entrada
    /// </summary>
    public static void Update(GameWindow gameWindow)
    {
        keyboardState = gameWindow.KeyboardState;
        mouseState = gameWindow.MouseState;
    }

    /// <summary>
    /// Retorna verdadeiro enquanto o usuário mantiver pressionada a tecla identificada pelo parâmetro de enumeração KeyCode.
    /// </summary>
    public static bool GetKey(KeyCode key)
    {
        return keyboardState.IsKeyDown((Keys)key);
    }

    /// <summary>
    /// Retorna verdadeiro durante o frame em que o usuário começa a pressionar a tecla identificada pelo parâmetro de enumeração KeyCode.
    /// </summary>
    public static bool GetKeyDown(KeyCode key)
    {
        return keyboardState.IsKeyPressed((Keys)key);
    }

    /// <summary>
    /// Retorna verdadeiro durante o frame em que o usuário solta a tecla identificada pelo parâmetro de enumeração KeyCode.
    /// </summary>
    public static bool GetKeyUp(KeyCode key)
    {
        return keyboardState.IsKeyReleased((Keys)key);
    }

    /// <summary>
    /// Retorna verdadeiro durante o frame em que o usuário pressionar duas vezes a tecla identificada pelo parâmetro de enumeração KeyCode.
    /// </summary>
    /// <param name="key">Tecla a ser verificada</param>
    /// <returns>True se foi detectado double click neste frame</returns>
    public static bool GetKeyDouble(KeyCode key)
    {
        return CheckDoubleClick(key, GetKeyDown);
    }
    
    /// <summary>
    /// Retorna se o botão do mouse especificado está pressionado.
    /// </summary>
    public static bool GetMouseButton(KeyCode key)
    {
        return mouseState.IsButtonDown((MouseButton)key);
    }
    
    /// <summary>
    /// Retorna se o botão do mouse especificado está pressionado.
    /// </summary>
    public static bool GetMouseButtonDown(KeyCode key)
    {
        return mouseState.IsButtonPressed((MouseButton)key);
    }
    
    /// <summary>
    /// Retorna se o botão do mouse especificado está pressionado.
    /// </summary>
    public static bool GetMouseButtonUp(KeyCode key)
    {
        return mouseState.IsButtonReleased((MouseButton)key);
    }

    /// <summary>
    /// Retorna se o botão do mouse especificado está pressionado.
    /// </summary>
    public static bool GetMouseButtonDouble(KeyCode key)
    {
        return CheckDoubleClick(key, GetMouseButtonDown);
    }
    
    private static bool CheckDoubleClick(KeyCode key, Func<KeyCode, bool> getDownMethod)
    {
        // Inicializa o contador de tempo se esta tecla não existir no dicionário
        if (!lastClickTimes.ContainsKey(key))
        {
            lastClickTimes[key] = 0;
        }

        // Decrementa o tempo para esta tecla especifica (contagem regressiva)
        if (lastClickTimes[key] > 0)
        {
            lastClickTimes[key] -= Time.deltaTime;

            // Garante que o tempo não fique negativo
            if (lastClickTimes[key] < 0)
            {
                lastClickTimes[key] = 0;
            }
        }

        // Se a tecla foi pressionada neste frame
        if (getDownMethod(key))
        {
            // Se ainda esta no tempo do double clique (segundo clique)
            if (lastClickTimes[key] > 0)
            {
                // Resta o timer após detectar o double click
                lastClickTimes[key] = 0;

                // Double click detectado!
                return true;
            }
            else
            {
                // Primeiro Click - Iniciar o timer;
                lastClickTimes[key] = DOUBLE_CLICK_TIME;
            }
        }

        // Não foi double click
        return false;
    }
}
