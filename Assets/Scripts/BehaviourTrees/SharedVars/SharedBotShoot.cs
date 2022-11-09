using System;
using BehaviorDesigner.Runtime;
using Bots;

namespace BehaviourTrees.SharedVars
{
    [Serializable]
    public class SharedBotShoot : SharedVariable<BotShoot>
    {
        public static implicit operator SharedBotShoot(BotShoot value) 
            => new SharedBotShoot { Value = value };
    }
}
