using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using PlayerAbilities;
using UnityEngine;

namespace BehaviourTrees
{
    [TaskDescription("Check to target setted.")]
    [TaskCategory("Movement")]
    public class PlayerFounded : Conditional
    {
        public SharedGameObject ReturnedObject;

        public override TaskStatus OnUpdate()
        {
            if (ReturnedObject.Value != null)
            {
                return TaskStatus.Success;
            }
            
            return TaskStatus.Failure;
        }
    }
}