using BehaviorDesigner.Runtime.Tasks;

namespace BehaviourTrees
{
    public class StopMovement : Action 
    {
        public SharedBotNavMesh SelfBotNavMesh;

        public override TaskStatus OnUpdate()
        {
            SelfBotNavMesh.Value.Stop();
            return TaskStatus.Success;
        }
    }
}
