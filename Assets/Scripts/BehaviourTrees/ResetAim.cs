using BehaviorDesigner.Runtime.Tasks;
using BehaviourTrees.SharedVars;

namespace BehaviourTrees
{
    [TaskCategory("Movement")]
    public class ResetAim : Action 
    {
        public SharedBotAim SelfBotAim;

        public override TaskStatus OnUpdate()
        {
            SelfBotAim.Value.ResetVerticalAim();
            return TaskStatus.Success;
        }
    }
}
