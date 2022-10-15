using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Match
{
    [RequireComponent(typeof(TMP_Text))]
    public class MatchTimer : MonoBehaviour
    {
        private readonly string _pattern = "{0:00}:{1:00}";
        
        private TMP_Text _text;
        private bool _isTimerStop = false;

        [SerializeField] private float _matchTimeInSeconds;
        [SerializeField] private MatchEndScoreboard _matchScoreboard;
        [SerializeField] private TimeSyncronizer _syncronizer;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            if(_isTimerStop == true)
            {
                return;
            }

            if(_matchTimeInSeconds > 0)
            {
                _matchTimeInSeconds -= Time.deltaTime;
                UpdateTimer();
            }
            else
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            _matchScoreboard.OpenPanel();
            _text.text = string.Format(_pattern, 0, 00);
            _isTimerStop = true;
        }

        private void UpdateTimer()
        {
            int minutes = Mathf.FloorToInt(_matchTimeInSeconds / 60);
            int seconds = Mathf.FloorToInt(_matchTimeInSeconds % 60);

            _text.text = string.Format(_pattern, minutes, seconds);
        }
    }
}
