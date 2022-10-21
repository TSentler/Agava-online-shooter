using UnityEngine;

public class DebugDrawSphere : MonoBehaviour
{
    [SerializeField] private float _radius = 0.05f;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
