using BehaviorDesigner.Runtime.Tasks;
using BehaviourTrees.SharedVars;

namespace BehaviourTrees
{
    [TaskCategory("Movement")]
    public class StopMovement : Action 
    {
        public SharedBotMovement SelfBotMovement;

        public override TaskStatus OnUpdate()
        {
            SelfBotMovement.Value.Stop();
            return TaskStatus.Success;
        }
    }
}
