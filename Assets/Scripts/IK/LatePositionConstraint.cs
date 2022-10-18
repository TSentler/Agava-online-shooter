using UnityEngine;

public class LatePositionConstraint : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    
    private void LateUpdate()
    {
        var parent = transform.parent;
        var scale = transform.localScale;
        transform.parent = _target;
        transform.localPosition = _offset;
        transform.parent = parent;
        transform.localScale = scale;
    }
}
