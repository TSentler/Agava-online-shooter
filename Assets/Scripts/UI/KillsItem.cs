using TMPro;
using UnityEngine;

public class KillsItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _killerName;
    [SerializeField] private TMP_Text _deadPlayerName;

    private float _timeElapsed = -1f;
    
    public void Initialize(string killerName, string deadPlayerName, float delay)
    {
        _killerName.text = killerName;
        _deadPlayerName.text = deadPlayerName;
        _timeElapsed = delay;
    }
    
    private void Update()
    {
        if (_timeElapsed > 0f)
        {
            _timeElapsed -= Time.deltaTime;
            if (_timeElapsed <= 0f)
            {
                DisableKillItem();
            }
        } 
    }

    private void DisableKillItem()
    {
        Destroy(gameObject);
    }
}
