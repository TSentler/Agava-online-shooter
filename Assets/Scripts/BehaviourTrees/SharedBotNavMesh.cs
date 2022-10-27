using System;
using BehaviorDesigner.Runtime;
using Bots;

namespace BehaviourTrees
{
    [Serializable]
    public class SharedBotNavMesh : SharedVariable<BotNavMesh>
    {
        public static implicit operator SharedBotNavMesh(BotNavMesh value) 
            => new SharedBotNavMesh { Value = value };
    }
}
