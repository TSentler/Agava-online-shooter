using UnityEngine;

namespace Unity
{
    [DisallowMultipleComponent]
    public class IndestructibleGameObject : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
