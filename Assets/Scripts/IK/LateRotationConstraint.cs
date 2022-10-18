using UnityEngine;

public class LateRotationConstraint : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    
    private void LateUpdate()
    {
        var parent = transform.parent;
        var scale = transform.localScale;
        transform.parent = _target;
        transform.localRotation = Quaternion.Euler(_offset);
        transform.parent = parent;
        transform.localScale = scale;
    }
}
