using UnityEngine;

public class UniqNetworkGameObject : MonoBehaviour
{
    private static UniqNetworkGameObject _instance;

    [SerializeField] private GameObject[] _hiden;
    
    private void Awake()
    {
        if (_instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            _instance = this;
            for (int i = 0; i < _hiden.Length; i++)
            {
                _hiden[i].SetActive(true);
            }
            DontDestroyOnLoad(this);
        }
    }
}
