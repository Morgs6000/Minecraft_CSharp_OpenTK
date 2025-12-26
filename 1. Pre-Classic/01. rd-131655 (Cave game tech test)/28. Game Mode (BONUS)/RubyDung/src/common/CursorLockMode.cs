namespace RubyDung.common;

/// <summary>
/// Como o cursor deve se comportar.
/// </summary>
public enum CursorLockMode
{
    /// <summary>
    /// O comportamento do cursor permanece inalterado.
    /// </summary>
    None,

    /// <summary>
    /// Fixa o cursor no centro da tela de jogo.
    /// </summary>
    Locked,

    /// <summary>
    /// Limitar o cursor à janela do jogo.
    /// </summary>
    Confined
}
