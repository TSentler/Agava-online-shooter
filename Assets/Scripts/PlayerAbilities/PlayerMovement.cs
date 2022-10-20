using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerAbilities
{
    [RequireComponent(typeof(PhotonView),
        typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour 
    {
        private PhotonView _photonView;
        private CharacterController _character;

        [SerializeField] private float _speed;

        public event UnityAction<Vector3> DirectionChanged;
        
        private void Awake()
        {
            _character = GetComponent<CharacterController>();
            _photonView = GetComponent<PhotonView>();
            _photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        private void Update()
        {
            if (_photonView.IsMine == false)
                return;

            var inputDirection = GetInputDirection();
            var moveDirection = GetMoveDirection(inputDirection);
            var distance = moveDirection * _speed;
            _character.SimpleMove(distance);
            DirectionChanged?.Invoke(inputDirection);
        }

        private Vector3 GetInputDirection()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            return Vector3.right * horizontalInput + 
                   Vector3.forward * verticalInput;
        }
        
        private Vector3 GetMoveDirection(Vector3 inputDirection)
        {
            Vector3 moveDirectionForward = transform.forward * inputDirection.z;
            Vector3 moveDirectionSide = transform.right * inputDirection.x;

            return (moveDirectionForward + moveDirectionSide).normalized;
        }
    }
}
