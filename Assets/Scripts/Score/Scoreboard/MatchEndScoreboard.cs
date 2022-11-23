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
        [SerializeField] private CanvasGroup _matchEndPanel;
        [SerializeField] private Transform _fartherPanel;
        [SerializeField] private ScoreboardItem _scoreTemplate;
        [SerializeField] private TMP_Text _textTimer;
        [SerializeField] private float _maxTime;
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private OpenMenu _openMenu;

        private Dictionary<Player, int> _sortedScores = new Dictionary<Player, int>();
        private MatchmakingCallbacksCatcher _matchCallbacks;
        private bool _isTimerStope = false;
        private float _currentTime;
        private LeanLocalization _leanLocalization;
        private bool _canPlay = true;
        private bool _isAdOpened = false;

        public bool CanPlay => _canPlay;

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
            //_isAdOpened = true;
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
            _matchEndPanel.alpha = 0f;
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

        private void Start()
        {
#if YANDEX_GAMES
            Agava.YandexGames.InterstitialAd.Show(_adOpened, _adClosed, _adError, _adOfline);
#endif
        }

        private void Update()
        {
            if (_isAdOpened == true)
            {
                _canPlay = false;
            }

            Debug.Log(_isAdOpened);

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
            _matchEndPanel.alpha = 1f;
            MatchComplete?.Invoke();

            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.ContainsKey("Kills"))
                {
                    _sortedScores.Add(player, (int)player.CustomProperties["Kills"]);
                }
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

        public void OnExitInReloudButtonClick()
        {
#if YANDEX_GAMES
            Agava.YandexGames.InterstitialAd.Show(_adOpened, _adClosed, _adError, _adOfline);
#elif VK_GAMES && !UNITY_EDITOR
            Agava.VKGames.Interstitial.Show();
#elif CRAZY_GAMES
            CrazyAds.Instance.beginAdBreak(AdBreakCompletedCallback, AdErrorCallback);
#endif
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
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.CurrentRoom.CustomProperties.Clear();
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
#elif YANDEX_GAMES
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.CurrentRoom.CustomProperties.Clear();
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
            //Agava.YandexGames.InterstitialAd.Show(_adOpened, _adClosed, _adError, _adOfline);
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
            _isAdOpened = false;
            Cursor.lockState = CursorLockMode.Locked;
            _canPlay = true;
            Debug.Log("Error");
        }

        private void OnAdOfline()
        {
            _isAdOpened = false;
            _canPlay = true;
            Cursor.lockState = CursorLockMode.Locked;
;
        }

        private void OnAdClose(bool obj)
        {
            _isAdOpened = false;
            _canPlay = true;
            _openMenu.Close();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnAdOpen()
        {
            _isAdOpened = true;
            _canPlay = false;
            Cursor.lockState = CursorLockMode.None;
            _openMenu.Open();
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