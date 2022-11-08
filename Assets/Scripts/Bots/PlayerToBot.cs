using System;
using PlayerAbilities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Bots
{
    public class PlayerToBot : MonoBehaviour
    {
        [SerializeField] private PlayerInfo _playerInfo;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private BotInput _botInput;
        [SerializeField] private MouseLook _mouseLook;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private MovementPresenter _movementPresenter;
        [SerializeField] private GameObject[] _hide;
        [SerializeField] private UnityEvent _onConvertToBot;
            
        private void Awake()
        {
            if (_playerInfo.IsBot)
            {
                ToBot();
            }
        }

        public void ToBot()
        {
            var isBot = true;
            _mouseLook.enabled = isBot == false;
            _navMeshAgent.enabled = isBot;

            _movementPresenter.Initialize(_botInput);
            _playerMovement.Initialize(_botInput);
            
            for (int i = 0; i < _hide.Length; i++)
            {
                _hide[i].SetActive(isBot == false);
            }

            if (isBot)
            {
                _onConvertToBot.Invoke();
            }
        }
    }
}
