using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Score.Enemy
{
    [RequireComponent(typeof(EnemyMover))]
    public class EnemyScore : MonoBehaviour
    {
        private ScoreCounter _score;
        private EnemyMover _enemyMover;

        [SerializeField] private int _count = 1;

        private void Awake()
        {
            _score = FindObjectOfType<ScoreCounter>();
            _enemyMover = GetComponent<EnemyMover>();
        }

        private void OnEnable()
        {
            _enemyMover.OnDie += DieHandler;
        }

        private void OnDisable()
        {
            _enemyMover.OnDie -= DieHandler;
        }

        private void DieHandler()
        {
            _score.SetScore(_count);
        }
    }
}
