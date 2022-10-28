using System;
using Photon.Pun;
using UnityEngine;

[Serializable]
public class HumanBone
{
    [SerializeField] private HumanBodyBones _bone;
    [Range(0f,1f), SerializeField] private float _weight = 1f;
    
    public HumanBodyBones Bone => _bone;
    public float Weight => _weight;
}

public class AimIK : MonoBehaviour
{
    private float _iterations = 10f;
    private Transform[] _boneTransforms;

    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _target, _aim;
    [Range(0f,1f), SerializeField] private float _weight = 1f;
    [SerializeField] private float _angleLimit = 60f;
    [SerializeField] private HumanBone[] _humanBones;

    private float _oldWeight;

    private void Awake()
    {
        _oldWeight = _weight;
    }

    private void Start()
    {
        _boneTransforms = new Transform[_humanBones.Length];
        for (int i = 0; i < _boneTransforms.Length; i++)
        {
            _boneTransforms[i] = _animator.GetBoneTransform(
                _humanBones[i].Bone);
        }
    }

    public void WeightOff()
    {
        _weight = 0f;
    }

    public void WeightReset()
    {
        _weight = _oldWeight;
    }
    
    private Vector3 GetTargetPosition()
    {
        var targetDirection = _target.position - _aim.position;
        var aimDirecion = _aim.forward;
        
        var targetAngle = Vector3.Angle(targetDirection, aimDirecion);
        if (targetAngle > _angleLimit && _angleLimit != 0f)
        {
            var diff = targetAngle - _angleLimit;
            diff *= Mathf.Sign(targetDirection.y);
            targetDirection = Quaternion.AngleAxis(diff, _target.right) * targetDirection;
        }
        return targetDirection;
    }

    private void LateUpdate()
    {
        RotateBones();
    }
    
    
    private void RotateBones()
    {
        var targetPosition = GetTargetPosition();
        for (int i = 0; i < _iterations; i++)
        {
            for (int j = 0; j < _boneTransforms.Length; j++)
            {
                var boneTransform = _boneTransforms[j];
                if (boneTransform == null)
                    continue;
                
                AimAtTarget(boneTransform, targetPosition, 
                    _humanBones[j].Weight * _weight);
            }
        }
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        var aimDirection = _aim.forward;
        var targetDirection = targetPosition;
        var aimTowards = Quaternion.FromToRotation(
            aimDirection, targetDirection);
        var blendedRotation = Quaternion.Slerp(
            Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }
}
