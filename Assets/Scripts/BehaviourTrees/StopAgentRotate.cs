using BehaviorDesigner.Runtime.Tasks;
using BehaviourTrees.SharedVars;

namespace BehaviourTrees
{
    [TaskCategory("Movement")]
    public class StopAgentRotate : Action 
    {
        public SharedBotAim SharedBotAim;

        public override TaskStatus OnUpdate()
        {
            SharedBotAim.Value.StopAgentRotate();
            return TaskStatus.Success;
        }
    }
}
