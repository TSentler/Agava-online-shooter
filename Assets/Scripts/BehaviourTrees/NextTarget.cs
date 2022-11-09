using BehaviorDesigner.Runtime.Tasks;
using BehaviourTrees.SharedVars;

namespace BehaviourTrees
{
    public class NextTarget : Action 
    {
        public SharedBotPatrol SelfBotPatrol;

        public override TaskStatus OnUpdate()
        {
            SelfBotPatrol.Value.NextTarget();
            return TaskStatus.Success;
        }
    }
}
