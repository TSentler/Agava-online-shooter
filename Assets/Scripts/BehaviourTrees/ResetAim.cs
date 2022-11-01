using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviourTrees
{
    [TaskCategory("Movement")]
    public class ResetAim : Action 
    {
        public SharedBotNavMesh SelfBotNavMesh;

        public override TaskStatus OnUpdate()
        {
            SelfBotNavMesh.Value.ResetVerticalAim();
            return TaskStatus.Success;
        }
    }
}
