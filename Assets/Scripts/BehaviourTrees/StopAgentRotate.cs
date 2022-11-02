using BehaviorDesigner.Runtime.Tasks;

namespace BehaviourTrees
{
    [TaskCategory("Movement")]
    public class StopAgentRotate : Action 
    {
        public SharedBotNavMesh SelfBotNavMesh;

        public override TaskStatus OnUpdate()
        {
            SelfBotNavMesh.Value.StopAgentRotate();
            return TaskStatus.Success;
        }
    }
}
