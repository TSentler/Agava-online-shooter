using System;
using BehaviorDesigner.Runtime;
using Bots;

namespace BehaviourTrees
{
    [Serializable]
    public class SharedBotInput : SharedVariable<BotInput>
    {
        public static implicit operator SharedBotInput(BotInput value) 
            => new SharedBotInput { Value = value };
    }
}
