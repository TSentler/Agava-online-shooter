using BehaviorDesigner.Runtime.Tasks;
using BehaviourTrees.SharedVars;

namespace BehaviourTrees
{
    public class GoToPatrolPoint : Action 
    {
        public SharedBotPatrol SelfBotPatrol;
        public SharedBotMovement SelfBotMovement;

        public override TaskStatus OnUpdate()
        {
            SelfBotMovement.Value.GoDestination(
                SelfBotPatrol.Value.TargetPointPosition);
            return TaskStatus.Success;
        }
    }
}
