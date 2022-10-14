using Network;
using Photon.Realtime;
using UnityEngine;

namespace Score.Network
{
    //[RequireComponent(typeof(ScoreCounter))]
    public class ScoreInitializer : MonoBehaviour
    {
        private PlayerEnteredRoomCatcher _catcher;
        //private ScoreCounter _counter;
        
        //private void Awake()
        //{
        //    _counter = GetComponent<ScoreCounter>();
        //    _catcher = FindObjectOfType<PlayerEnteredRoomCatcher>();
        //}

        //private void OnEnable()
        //{
        //    _catcher.OnPlayerEnter += PlayerEnterHandler;
        //}

        //private void OnDisable()
        //{
        //    _catcher.OnPlayerEnter -= PlayerEnterHandler;
        //}

        //private void PlayerEnterHandler(Player player)
        //{
        //    _counter.SyncScore(player);
        //}
    }
}
