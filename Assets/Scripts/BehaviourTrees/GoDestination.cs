using BehaviorDesigner.Runtime.Tasks;

namespace BehaviourTrees
{
    public class GoDestination : Action 
    {
        public SharedBotNavMesh SelfBotNavMesh;

        public override TaskStatus OnUpdate()
        {
            SelfBotNavMesh.Value.GoDestination();
            return TaskStatus.Success;
        }
    }
}
