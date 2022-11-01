using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviourTrees
{
    [TaskCategory("Movement")]
    public class AimTarget : Action 
    {
        public SharedBotNavMesh SelfBotNavMesh;
        public SharedGameObject Target;

        public override TaskStatus OnUpdate()
        {
            if (Target.Value == null)
            {
                return TaskStatus.Failure;
            }
            SelfBotNavMesh.Value.Aim(Target.Value);
            return TaskStatus.Success;
        }
    }
}
