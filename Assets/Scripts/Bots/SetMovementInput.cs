using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Bots
{
    public class SetMovementInput : Action 
    {
        public SharedBotInput SelfBotInput;
        public SharedVector2 Direction;

        public override TaskStatus OnUpdate()
        {
            SelfBotInput.Value.MovementInput = Direction.Value;
            return TaskStatus.Success;
        }
    }
}
