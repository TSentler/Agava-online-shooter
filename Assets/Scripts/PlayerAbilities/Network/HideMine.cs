using UnityEngine;

namespace PlayerAbilities.Network
{
    public class HideMine : MonoBehaviour
    {
        [SerializeField] private PlayerInfo _playerInfo;

        private void Start()
        {
            if (_playerInfo.IsMine && _playerInfo.IsBot == false)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
