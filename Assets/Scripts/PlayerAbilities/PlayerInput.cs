using CharacterInput;
using Photon.Pun;
using UnityEngine;

namespace PlayerAbilities
{
    [RequireComponent(typeof(PhotonView))]
    public class PlayerInput : MonoBehaviour, ICharacterInputSource
    {
        private PhotonView _photonView;
        
        public Vector2 MovementInput { get; private set; }
        public bool IsJumpInput { get; private set; }

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        private void Update()
        {
            if (_photonView.IsMine == false)
                return;
            
            IsJumpInput = Input.GetButtonDown("Jump");
            MovementInput = GetInputDirection();
        }
        
        private Vector2 GetInputDirection()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            return Vector2.right * horizontalInput + 
                   Vector2.up * verticalInput;
        }
    }
}
