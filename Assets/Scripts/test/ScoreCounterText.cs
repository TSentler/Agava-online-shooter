using TMPro;
using UnityEngine;

namespace Score
{
    [RequireComponent(typeof(TMP_Text))]
    public class ScoreCounterText : MonoBehaviour
    {
        private TMP_Text _text;
        
        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        public void SetScore(int value)
        {
            _text.text = value.ToString();
        }
    }
}
