using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;
using PlayerAbilities;
using UnityEngine;

namespace BehaviourTrees
{
    [TaskDescription("Check to see if the any objects are within sight of the agent.")]
    [TaskCategory("Movement")]
    [UnityEngine.HelpURL("https://www.opsive.com/support/documentation/behavior-designer-movement-pack/")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
    public class CanSeePlayer : Conditional
    {
        [UnityEngine.Tooltip("The LayerMask of the objects that we are searching for")]
        public LayerMask objectLayerMask;
        [UnityEngine.Tooltip("If using the object layer mask, specifies the maximum number of colliders that the physics cast can collide with")]
        public int maxCollisionCount = 100;
        [UnityEngine.Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
        public LayerMask ignoreLayerMask;
        [UnityEngine.Tooltip("The field of view angle of the agent (in degrees)")]
        public SharedFloat fieldOfViewAngle = 90;
        [UnityEngine.Tooltip("The distance that the agent can see")]
        public SharedFloat viewDistance = 1000;
        [UnityEngine.Tooltip("The raycast offset relative to the pivot position")]
        public SharedVector3 offset;
        [UnityEngine.Tooltip("The target raycast offset relative to the pivot position")]
        public SharedVector3 targetOffset;
        [UnityEngine.Tooltip("The object that is within sight")]
        public SharedGameObject returnedObject;

        private GameObject[] agentColliderGameObjects;
        private int[] originalColliderLayer;
        private Collider[] overlapColliders;
        private int ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");

        // Returns success if an object was found otherwise failure
        public override TaskStatus OnUpdate()
        {
            GameObject playerInSight = null;
            overlapColliders = new Collider[maxCollisionCount];
            
            var hitCount = Physics.OverlapSphereNonAlloc(
                transform.TransformPoint(offset.Value), viewDistance.Value, 
                overlapColliders, objectLayerMask, QueryTriggerInteraction.Ignore);
            
            if (hitCount > 0) {
#if UNITY_EDITOR
                if (hitCount == overlapColliders.Length) {
                    Debug.LogWarning("Warning: The hit count is equal to the max collider array size. This will cause objects to be missed. Consider increasing the max collision count size.");
                }
#endif
                float minAngle = Mathf.Infinity;
                for (int i = 0; i < hitCount; ++i)
                {
                    var target = overlapColliders[i].transform;
                    if (target.TryGetComponent(out PlayerInfo playerInfo) == false)
                        continue;
                    
                    var direction = target.TransformPoint(targetOffset.Value) 
                                    - transform.TransformPoint(offset.Value);
                    float angle = Vector3.Angle(direction, transform.forward);
                    if (angle > fieldOfViewAngle.Value / 2)
                        continue;
                    
                    Debug.Log(target.gameObject.name);
                    Debug.Log(angle);
                    if (angle < minAngle) {
                        minAngle = angle;
                        playerInSight = target.gameObject;
                    }
                }
            }
            
            if (playerInSight != null)
            {
                returnedObject = playerInSight;
                return TaskStatus.Success;
            }
            
            return TaskStatus.Failure;
        }

        // Reset the public variables
        public override void OnReset()
        {
            fieldOfViewAngle = 90;
            viewDistance = 1000;
            offset = Vector3.zero;
            targetOffset = Vector3.zero;
            ignoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        }

        // Draw the line of sight representation within the scene window
        public override void OnDrawGizmos()
        {
            MovementUtility.DrawLineOfSight(Owner.transform, offset.Value, 
                fieldOfViewAngle.Value, 0f, 
                viewDistance.Value, false);
        }

        public override void OnBehaviorComplete()
        {
            MovementUtility.ClearCache();
        }
    }
}