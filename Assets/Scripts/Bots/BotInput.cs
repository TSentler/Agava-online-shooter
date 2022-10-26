using CharacterInput;
using UnityEngine;

namespace Bots
{
    public class BotInput : MonoBehaviour, ICharacterInputSource
    {
        public Vector2 MovementInput { get; set; }
        public bool IsJumpInput { get; set; }
    }
}
