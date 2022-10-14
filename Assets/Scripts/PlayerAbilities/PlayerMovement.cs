using Photon.Pun;
using UnityEngine;

namespace PlayerAbilities
{
    public class PlayerMovement : MonoBehaviour 
    {
        private const string RunAnimation = "IsRun";
    
        private Animator _animator;
    
        [SerializeField] private float _speed;
        [SerializeField] private PhotonView _photonViev;
        [SerializeField] private CharacterController _characterController;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _photonViev.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        private void Update()
        {
            if (_photonViev.IsMine == false)
                return;
        
            float horizontalInput = Input.GetAxis ("Horizontal");
            float verticalInput = Input.GetAxis ("Vertical");

            Vector3 moveDirectionForward = transform.forward * verticalInput;
            Vector3 moveDirectionSide = transform.right * horizontalInput;

            Vector3 direction = (moveDirectionForward + moveDirectionSide).normalized;
            // Vector3 inverseDirection = transform.InverseTransformDirection(direction);
            Vector3 distance = direction * _speed * Time.deltaTime;
            if (direction != Vector3.zero)
            {
                _characterController.Move(distance);
                _animator.SetBool(RunAnimation, true);
            }
            else
            {
                _animator.SetBool(RunAnimation, false);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        
        }
    }
}
