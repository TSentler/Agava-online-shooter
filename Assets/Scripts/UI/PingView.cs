using Photon.Pun;
using UnityEngine;
using TMPro;

public class PingView : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private TMP_Text _pingText; 

    private float _timePerUpdatePing = 2f;
    private float _timeRemaining;

    private void Awake()
    {
        if (_photonView.IsMine == false)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _pingText.text = $"Ping: {PhotonNetwork.GetPing()}";
        _timeRemaining = _timePerUpdatePing;
    }

    private void Update()
    {
        _timeRemaining -= Time.deltaTime;

        if (_timeRemaining < 0)
        {
            _timeRemaining = _timePerUpdatePing;
            _pingText.text = $"Ping: {PhotonNetwork.GetPing()}";
        }
    }
}
