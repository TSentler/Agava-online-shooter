using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviourTrees.SharedVars;

namespace BehaviourTrees
{
    [TaskCategory("Attack")]
    public class Shoot: Action 
    {
        public SharedBotShoot SharedBotShoot;
        public SharedGameObject Target;

        public override TaskStatus OnUpdate()
        {
            if (Target.Value == null)
            {
                return TaskStatus.Failure;
            }

            SharedBotShoot.Value.Shoot(Target.Value.transform);
            return TaskStatus.Success;
        }
    }
}
