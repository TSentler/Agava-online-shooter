using BehaviorDesigner.Runtime.Tasks;

namespace BehaviourTrees
{
    public class NextTarget : Action 
    {
        public SharedBotNavMesh SelfBotNavMesh;

        public override TaskStatus OnUpdate()
        {
            SelfBotNavMesh.Value.NextTarget();
            return TaskStatus.Success;
        }
    }
}
