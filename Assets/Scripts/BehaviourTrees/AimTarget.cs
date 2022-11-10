using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviourTrees.SharedVars;

namespace BehaviourTrees
{
    [TaskCategory("Movement")]
    public class AimTarget : Action 
    {
        public SharedBotAim SelfBotNavMesh;
        public SharedGameObject Target;

        public override TaskStatus OnUpdate()
        {
            if (Target.Value == null)
            {
                return TaskStatus.Failure;
            }
            SelfBotNavMesh.Value.Aim(Target.Value.transform.position);
            return TaskStatus.Success;
        }
    }
}
