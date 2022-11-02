using BehaviorDesigner.Runtime.Tasks;

namespace BehaviourTrees
{
    [TaskCategory("Movement")]
    public class ResetAgentRotate : Action 
    {
        public SharedBotNavMesh SelfBotNavMesh;

        public override TaskStatus OnUpdate()
        {
            SelfBotNavMesh.Value.ResetAgentRotate();
            return TaskStatus.Success;
        }
    }
}
