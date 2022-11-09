using BehaviorDesigner.Runtime.Tasks;
using BehaviourTrees.SharedVars;

namespace BehaviourTrees
{
    [TaskCategory("Movement")]
    public class ResetAgentRotate : Action 
    {
        public SharedBotAim SelfBotNavMesh;

        public override TaskStatus OnUpdate()
        {
            SelfBotNavMesh.Value.ResetAgentRotate();
            return TaskStatus.Success;
        }
    }
}
