using System;
using BehaviorDesigner.Runtime;
using Bots;

namespace BehaviourTrees.SharedVars
{
    [Serializable]
    public class SharedBotMovement : SharedVariable<BotMovement>
    {
        public static implicit operator SharedBotMovement(BotMovement value) 
            => new SharedBotMovement { Value = value };
    }
}
