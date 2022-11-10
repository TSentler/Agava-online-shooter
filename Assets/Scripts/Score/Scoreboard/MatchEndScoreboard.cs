using Network;
using Network.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Score
{
    public class MatchEndScoreboard : MonoBehaviour
    {
        [SerializeField] private GameObject _matchEndPanel;
        [SerializeField] private Transform _fartherPanel;
        [SerializeField] private ScoreboardItem _scoreTemplate;
        [SerializeField] private TMP_Text _textTimer;
        [SerializeField] private float _maxTime;
        [SerializeField] private PhotonView _photonView;

        private Dictionary<Player, int> _sortedScores = new Dictionary<Player, int>();
        private MatchmakingCallbacksCatcher _matchCallbacks;
        private bool _isTimerStope = false;    
        private float _currentTime;

        public Action MatchComplete;

        private void Awake()
        {
            _matchCallbacks = FindObjectOfType<MatchmakingCallbacksCatcher>();
            _sortedScores.Clear();
            PhotonNetwork.AutomaticallySyncScene = true;

            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;
        }

        private void OnEnable()
        {
            _matchCallbacks.OnRoomLeft += RoomLeftHandler;
            _matchEndPanel.SetActive(false);
        }

        private void OnDisable()
        {
            _matchCallbacks.OnRoomLeft -= RoomLeftHandler;
        }

        private void Update()
        {
            if (_isTimerStope == false)
                return;

            _currentTime -= Time.deltaTime;
            _textTimer.text = "Reload level in " + _currentTime.ToString("0") + " seconds";

            if(_currentTime <= 0)
            {
                _textTimer.text = "Reload level";
             
                if (PhotonNetwork.IsMasterClient)
                {
                    _photonView.RPC(nameof(ReloadLevel), RpcTarget.All);
                }

                _isTimerStope = false;
            }
        }

        public void OpenPanel()
        {
            _matchEndPanel.SetActive(true);
            MatchComplete?.Invoke();

            foreach (var player in PhotonNetwork.PlayerList)
            {
                _sortedScores.Add(player, (int)player.CustomProperties["Kills"]);
            }

            var scoreSort = _sortedScores.OrderByDescending(x => x.Value).ThenBy(y => y.Key.CustomProperties["Death"]);

            foreach (var score in scoreSort)
            {
                ScoreboardItem item = Instantiate(_scoreTemplate, _fartherPanel);
                item.Initialize(score.Key);
            }

            Cursor.lockState = CursorLockMode.None;
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;
            _isTimerStope = true;
            _currentTime = _maxTime;
        }

        public void OnRestartButtonClick()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.CurrentRoom.CustomProperties.Clear();
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnExitButtonClick()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void RoomLeftHandler()
        {
            PhotonNetwork.LoadLevel(0);
        }

        [PunRPC]
        private void ReloadLevel()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.CurrentRoom.CustomProperties.Clear();
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }
    }
}