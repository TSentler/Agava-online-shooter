using UnityEngine;

namespace CharacterInput
{
    public interface ICharacterInputSource
    {
        Vector2 MovementInput { get; }
        Vector2 MouseInput { get; }
        bool IsJumpInput { get; }
    }
}
