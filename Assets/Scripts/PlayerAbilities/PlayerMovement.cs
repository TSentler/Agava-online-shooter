using Photon.Pun;
using UnityEngine;

namespace PlayerAbilities
{
    [RequireComponent(typeof(PhotonView),
        typeof(Animator),
        typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour 
    {
        private readonly string _isRunName = "IsRun",
            _inputForwardName = "InputForward",
            _inpurRightName = "InputRight";
    
        private Animator _animator;
        private PhotonView _photonView;
        private CharacterController _characterController;
    
        [SerializeField] private float _speed;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
            _photonView = GetComponent<PhotonView>();
            _photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        private void Update()
        {
            if (_photonView.IsMine == false)
                return;
            
            var direction = GetDirection();
            var distance = direction * _speed * Time.deltaTime;
            _characterController.Move(distance);
            var isRun = direction != Vector3.zero;
            _animator.SetBool(_isRunName, isRun);
        }

        private Vector3 GetDirection()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            _animator.SetFloat(_inputForwardName, verticalInput);
            _animator.SetFloat(_inpurRightName, horizontalInput);

            Vector3 moveDirectionForward = transform.forward * verticalInput;
            Vector3 moveDirectionSide = transform.right * horizontalInput;

            return (moveDirectionForward + moveDirectionSide).normalized;
        }
    }
}
