using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SetHandIKPosition : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private bool _ikActive = false;
    [Range(0f,1f), SerializeField] private float _weight = 1f;
    [SerializeField] private Transform _handTarget = null;
    [SerializeField] private AvatarIKGoal _ikGoal;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_ikActive == false)
        {
            SetIKWeight(0f);
            return;
        }

        SetIKWeight(_weight);
        _animator.SetIKPosition(_ikGoal, _handTarget.position);
    }

    private void SetIKWeight(float weight)
    {
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
    }
}
