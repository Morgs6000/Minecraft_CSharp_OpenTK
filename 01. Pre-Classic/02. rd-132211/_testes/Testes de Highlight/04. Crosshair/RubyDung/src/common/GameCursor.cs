using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace RubyDung.common;

/// <summary>
/// API Cursor para definir o cursor (ponteiro do mouse).
/// </summary>
public class GameCursor
{
    /// <summary>
    /// Determina se o ponteiro de hardware está fixo no centro da visualização, restrito à janela ou não está restrito.
    /// </summary>
    public static CursorLockMode lockState;

    private static CursorState _lastAppliedState;

    public static void Update(GameWindow gameWindow)
    {
        CursorState newState = CursorState.Normal;

        switch (lockState)
        {
            case CursorLockMode.None:
                newState = CursorState.Normal;
                break;
            case CursorLockMode.Locked:
                newState = CursorState.Grabbed;
                break;
            case CursorLockMode.Confined:
                newState = CursorState.Confined;
                break;
        }

        if (newState != _lastAppliedState)
        {
            gameWindow.CursorState = newState;
            _lastAppliedState = newState;
        }
    }
}
