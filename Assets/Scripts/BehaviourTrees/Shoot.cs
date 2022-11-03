using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BehaviourTrees
{
    [TaskCategory("Attack")]
    public class Shoot: Action 
    {
        public SharedBotNavMesh SelfBotNavMesh;
        public SharedGameObject Target;
        public SharedVector3 Offset;
        public SharedVector3 TargetOffset;

        public override TaskStatus OnUpdate()
        {
            if (Target.Value == null)
            {
                return TaskStatus.Failure;
            }

            var startPoint = transform.TransformPoint(Offset.Value);
            var endPoint =
                Target.Value.transform.TransformPoint(TargetOffset.Value);
            var direction = endPoint - startPoint;
            SelfBotNavMesh.Value.Shoot(direction);
            return TaskStatus.Success;
        }
    }
}
