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
        private CharacterController _characterController;

        [SerializeField] private float _speed;

        public event UnityAction<Vector3> DirectionChanged;
        
        private void Awake()
        {
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
            DirectionChanged?.Invoke(direction);
        }

        private Vector3 GetDirection()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 moveDirectionForward = transform.forward * verticalInput;
            Vector3 moveDirectionSide = transform.right * horizontalInput;

            return (moveDirectionForward + moveDirectionSide).normalized;
        }
    }
}
