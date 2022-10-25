using Photon.Pun;
using UnityEngine;

namespace PlayerAbilities
{
    [RequireComponent(typeof(PhotonView),
        typeof(PlayerMovement))]
    public class PlayerInput : MonoBehaviour
    {
        private PhotonView _photonView;
        private PlayerMovement _movement;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _photonView = GetComponent<PhotonView>();
            _photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        private void Update()
        {
            if (_photonView.IsMine == false)
                return;
            
            var isJump = Input.GetButtonDown("Jump");
            var inputDirection = GetInputDirection();
            _movement.Move(inputDirection, isJump);
        }
        
        private Vector3 GetInputDirection()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            return Vector3.right * horizontalInput + 
                   Vector3.forward * verticalInput;
        }
    }
}
