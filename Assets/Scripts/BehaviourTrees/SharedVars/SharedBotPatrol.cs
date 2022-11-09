using System;
using BehaviorDesigner.Runtime;
using Bots;

namespace BehaviourTrees.SharedVars
{
    [Serializable]
    public class SharedBotPatrol : SharedVariable<BotPatrol>
    {
        public static implicit operator SharedBotPatrol(BotPatrol value) 
            => new SharedBotPatrol { Value = value };
    }
}
