using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviourTrees
{
    [TaskCategory("Attack")]
    public class Shoot: Action 
    {
        public SharedBotNavMesh SelfBotNavMesh;
        public SharedGameObject Target;

        public override TaskStatus OnUpdate()
        {
            if (Target.Value == null)
            {
                return TaskStatus.Failure;
            }

            SelfBotNavMesh.Value.Shoot(Target.Value.transform);
            return TaskStatus.Success;
        }
    }
}
