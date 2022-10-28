using System;
using Photon.Pun;
using UnityEngine;

namespace PlayerAbilities
{
    [RequireComponent(typeof(PhotonView))]
    public class PlayerInfo : MonoBehaviour
    {
        [SerializeField] private bool _isBot;
        [SerializeField] private PhotonView _photonView;

        private string _botNickName;

        public PhotonView PhotonView => _photonView;
        public bool IsMine => _photonView.IsMine;
        public bool IsMineBot => IsMine && IsBot;
        public bool IsBot => _isBot;
        public string NickName => GetNickName();

        private string GetNickName()
        {
            if (IsBot)
            {
                if (_botNickName == string.Empty)
                {
                    _botNickName = GenerateNickName();
                }
                return _botNickName;
            }

            return PhotonNetwork.NickName;
        }

        private string GenerateNickName()
        {
            return "Bot";
        }
    }
}
