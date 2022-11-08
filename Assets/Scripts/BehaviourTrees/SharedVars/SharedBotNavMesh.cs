using System;
using BehaviorDesigner.Runtime;
using Bots;

namespace BehaviourTrees.SharedVars
{
    [Serializable]
    public class SharedBotAim : SharedVariable<BotAim>
    {
        public static implicit operator SharedBotAim(BotAim value) 
            => new SharedBotAim { Value = value };
    }
}
