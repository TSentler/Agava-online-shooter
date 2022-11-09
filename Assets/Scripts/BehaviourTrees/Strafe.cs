using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviourTrees.SharedVars;
using UnityEngine;

namespace BehaviourTrees
{
    [TaskCategory("Movement")]
    public class Strafe : Action 
    {
        public SharedBotMovement BotMovement;
        public SharedGameObject Target;
        public SharedFloat Distance;
        public SharedGameObject Root;

        public override TaskStatus OnUpdate()
        {
            var targetTransform = Target.Value.transform;
            var rootPosition = Root.Value.transform.position;
            var offset = targetTransform.position - rootPosition;
            var sign = Random.Range(0, 2) * 2 - 1; 
            var dir = Vector3.Cross(offset, Vector3.up);
            BotMovement.Value.GoDestination(rootPosition +
                                            sign * dir * Distance.Value);
            return TaskStatus.Success;
        }
    }
}
