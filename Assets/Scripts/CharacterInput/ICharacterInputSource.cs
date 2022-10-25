using UnityEngine;

namespace CharacterInput
{
    public interface ICharacterInputSource
    {
        Vector2 MovementInput { get; }
        bool IsJumpInput { get; }
    }
}
