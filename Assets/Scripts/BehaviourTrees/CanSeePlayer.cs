using System.Collections.Generic;
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
        public LayerMask ObjectLayerMask;
        [UnityEngine.Tooltip("If using the object layer mask, specifies the maximum number of colliders that the physics cast can collide with")]
        public int MaxCollisionCount = 100;
        [UnityEngine.Tooltip("The LayerMask of the objects to ignore when performing the line of sight check")]
        public LayerMask IgnoreLayerMask;
        [UnityEngine.Tooltip("The field of view angle of the agent (in degrees)")]
        public SharedFloat FieldOfViewAngle = 90;
        [UnityEngine.Tooltip("The distance that the agent can see")]
        public SharedFloat ViewDistance = 1000;
        [UnityEngine.Tooltip("The raycast offset relative to the pivot position")]
        public SharedVector3 Offset;
        [UnityEngine.Tooltip("The target raycast offset relative to the pivot position")]
        public SharedVector3 TargetOffset;
        public SharedBool IsRaycast = true;
        [UnityEngine.Tooltip("The object that is within sight")]
        public SharedGameObject ReturnedObject;

        private GameObject[] _agentColliderGameObjects;
        private int[] _originalColliderLayer;
        private Collider[] _overlapColliders;
        private int _ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");

        public override TaskStatus OnUpdate()
        {
            _overlapColliders = new Collider[MaxCollisionCount];
            
            ReturnedObject.Value = FindPlayerInSight();
            if (ReturnedObject.Value != null)
            {
                Debug.Log(ReturnedObject.Value.name);
                return TaskStatus.Success;
            }
            
            return TaskStatus.Failure;
        }

        private GameObject FindPlayerInSight()
        {
            GameObject playerInSight = null;
            var startPoint = transform.TransformPoint(Offset.Value);
            List<GameObject> playersInSector = new();
            var hitCount = Physics.OverlapSphereNonAlloc(startPoint, ViewDistance.Value,
                _overlapColliders, ObjectLayerMask, QueryTriggerInteraction.Ignore);

            if (hitCount > 0)
            {
#if UNITY_EDITOR
                if (hitCount == _overlapColliders.Length)
                {
                    Debug.LogWarning(
                        "Warning: The hit count is equal to the max collider array size. This will cause objects to be missed. Consider increasing the max collision count size.");
                }
#endif
                float minAngle = Mathf.Infinity;
                for (int i = 0; i < hitCount; ++i)
                {
                    var target = _overlapColliders[i].transform;
                    
                    if (target.TryGetComponent(out PlayerInfo playerInfo) == false)
                        continue;

                    var angle = CalculateAngle(target);
                    if (angle > FieldOfViewAngle.Value / 2)
                        continue;

                    var endPoint = target.TransformPoint(TargetOffset.Value);
                    bool isWithinSight = IsRaycast.Value == false ||
                        WithinSight(startPoint, endPoint, target.gameObject, 
                            IgnoreLayerMask, true);
                    if (isWithinSight && angle < minAngle)
                    {
                        minAngle = angle;
                        playerInSight = target.gameObject;
                    }
                }
            }

            return playerInSight;
        }

        private bool WithinSight(Vector3 startPosition, Vector3 endPosition,
            GameObject targetObject, int ignoreLayerMask, bool drawDebugRay)
        {
            if (targetObject == null || targetObject.activeSelf == false)
                return false;

            var hitTransform = LineOfSight(startPosition, endPosition, 
                ignoreLayerMask);
            if (hitTransform == null 
                || MovementUtility.IsAncestor(targetObject.transform, hitTransform))
            {
                return true;
            }
            
#if UNITY_EDITOR
            if (drawDebugRay) {
                Debug.DrawLine(startPosition, endPosition, Color.red);
            }
#endif
            return false;
        }

        private Transform LineOfSight(Vector3 startPositiont, Vector3 endPosition, 
            int ignoreLayerMask)
        {
            Transform hitTransform = null;
            if (Physics.Linecast(startPositiont, endPosition, out var hit, 
                    ~ignoreLayerMask, QueryTriggerInteraction.Ignore)) 
            {
                hitTransform = hit.transform;
            }
            return hitTransform;
        }
        
        private float CalculateAngle(Transform target)
        {
            var direction = target.TransformPoint(TargetOffset.Value)
                            - transform.TransformPoint(Offset.Value);
            float angle = Vector3.Angle(direction, transform.forward);
            return angle;
        }

        // Reset the public variables
        public override void OnReset()
        {
            FieldOfViewAngle = 90;
            ViewDistance = 1000;
            Offset = Vector3.zero;
            TargetOffset = Vector3.zero;
            IgnoreLayerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
            IsRaycast = true;
        }

        // Draw the line of sight representation within the scene window
        public override void OnDrawGizmos()
        {
            MovementUtility.DrawLineOfSight(Owner.transform, Offset.Value, 
                FieldOfViewAngle.Value, 0f, 
                ViewDistance.Value, false);
        }

        public override void OnBehaviorComplete()
        {
            MovementUtility.ClearCache();
        }
    }
    
}