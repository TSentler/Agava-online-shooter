using BehaviorDesigner.Runtime.Tasks;

namespace BehaviourTrees
{
    public class SetPlayerDestinationTarget : Action 
    {
        public SharedBotNavMesh SelfBotNavMesh;

        public override TaskStatus OnUpdate()
        {
            SelfBotNavMesh.Value.NextTarget();
            return TaskStatus.Success;
        }
    }
}
