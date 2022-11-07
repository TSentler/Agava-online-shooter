using System;
using UnityEngine;

namespace Bots
{
    public class PlayerToBot : MonoBehaviour
    {
        public GameObject[] _toHide, _toShow;

        private void Awake()
        {
            for (int i = 0; i < _toHide.Length; i++)
            {
                _toHide[i].SetActive(false);
            }

            for (int i = 0; i < _toShow.Length; i++)
            {
                _toShow[i].SetActive(true);
            }
        }
    }
}
