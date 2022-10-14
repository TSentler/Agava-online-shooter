using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class MatchTimer : MonoBehaviour
{
    [SerializeField] private float _matchTimeInSeconds;

    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
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
        Debug.Log("Matsh is complete");
        _text.text = 0 + ":" + 00;
    }

    private void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(_matchTimeInSeconds / 60);
        int seconds = Mathf.FloorToInt(_matchTimeInSeconds % 60);

        _text.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }
}
