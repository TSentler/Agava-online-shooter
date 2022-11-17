using Lean.Localization;
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
        private LeanLocalization _leanLocalization;

        public Action MatchComplete;
        private Action _adOpened;
        private Action<bool> _adClosed;
        private Action _adOfline;
        private Action<string> _adError;
        private Action _adErrorVk;
        //private CrazyAds.AdBreakCompletedCallback AdBreakCompletedCallback;
        //private CrazyAds.AdErrorCallback AdErrorCallback;

        private void Awake()
        {
            _matchCallbacks = FindObjectOfType<MatchmakingCallbacksCatcher>();
            _sortedScores.Clear();
            _leanLocalization = FindObjectOfType<LeanLocalization>();
            PhotonNetwork.AutomaticallySyncScene = true;

            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;
        }

        private void OnEnable()
        {
            _matchCallbacks.OnRoomLeft += RoomLeftHandler;
            _matchEndPanel.SetActive(false);
            _adOpened += OnAdOpen;
            _adClosed += OnAdClose;
            _adOfline += OnAdOfline;
            _adError += OnAdError;
            //AdBreakCompletedCallback += OnCrazyGamesRevardedAd;
            //AdErrorCallback += OnCrazyGamesErrorAd;
        }

        private void OnDisable()
        {
            _matchCallbacks.OnRoomLeft -= RoomLeftHandler;
            _adOpened -= OnAdOpen;
            _adClosed -= OnAdClose;
            _adOfline -= OnAdOfline;
            _adError -= OnAdError;
            //AdBreakCompletedCallback -= OnCrazyGamesRevardedAd;
            //AdErrorCallback -= OnCrazyGamesErrorAd;
        }

        private void Update()
        {
            if (_isTimerStope == false)
                return;

            _currentTime -= Time.deltaTime;
            string reload = LeanLocalization.GetTranslationText("ReloudText");
            string seconds = LeanLocalization.GetTranslationText("Seconds");
            _textTimer.text = reload + _currentTime.ToString("0") + seconds;

            if (_currentTime <= 0)
            {
                string reloading = LeanLocalization.GetTranslationText("Relouding");
                _textTimer.text = reloading;

                if (PhotonNetwork.IsMasterClient)
                {
                    _photonView.RPC(nameof(ReloadLevelRPC), RpcTarget.All);
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
            ReloadLevel();
        }

        public void OnExitButtonClick()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void RoomLeftHandler()
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(0);
        }

        [PunRPC]
        private void ReloadLevelRPC()
        {
#if !UNITY_WEBGL || UNITY_EDITOR

#elif YANDEX_GAMES
            Agava.YandexGames.InterstitialAd.Show(_adOpened, _adClosed, _adError, _adOfline);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.CurrentRoom.CustomProperties.Clear();
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
#elif VK_GAMES && !UNITY_EDITOR
            Agava.VKGames.Interstitial.Show();
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.CurrentRoom.CustomProperties.Clear();
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
#elif CRAZY_GAMES
            CrazyAds.Instance.beginAdBreak(AdBreakCompletedCallback, AdErrorCallback);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.CurrentRoom.CustomProperties.Clear();
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
#endif
            //_photonView.RPC(nameof(ReloadLevel), RpcTarget.All);
        }

        private void OnAdError(string obj)
        {
        }

        private void OnAdOfline()
        {
        }

        private void OnAdClose(bool obj)
        {
        }

        private void OnAdOpen()
        {
        }

        private void OnCrazyGamesRevardedAd()
        {
        }

        private void OnCrazyGamesErrorAd()
        {
        }

        [PunRPC]
        private void ReloadLevel()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.CurrentRoom.CustomProperties.Clear();
                PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}