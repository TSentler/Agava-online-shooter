using CharacterInput;
using Photon.Pun;
using UnityEngine;

namespace PlayerAbilities
{
    [RequireComponent(typeof(PlayerInfo))]
    public class PlayerInput : MonoBehaviour, ICharacterInputSource
    {
        private PlayerInfo _playerInfo;
        
        public Vector2 MovementInput { get; private set; }
        public Vector2 MouseInput { get; private set; }
        public bool IsJumpInput { get; private set; }

        private void Awake()
        {
            _playerInfo = GetComponent<PlayerInfo>();
            _playerInfo.PhotonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        private void Update()
        {
            if (_playerInfo.IsMine == false || _playerInfo.IsBot)
                return;
            
            IsJumpInput = Input.GetButtonDown("Jump");
            MovementInput = GetInputDirection();
            MouseInput = GetMouseInput();
        }

        private Vector2 GetMouseInput()
        {
            return new Vector2(Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y"));
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
