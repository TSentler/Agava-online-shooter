using System;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class KillFidPanel : MonoBehaviour
{
    [SerializeField] private KillsItem _killTemplate;
    [SerializeField] private Transform _fartherPanel;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private CanvasGroup _canvasGroup;

    private List<KillsItem> _killsTemplates = new List<KillsItem>();
    private Coroutine _disableCoroutine;
    private float _hideDelay = 3f;

    private void Update()
    {
        if (_killsTemplates.Count > 0
                && _killsTemplates[_killsTemplates.Count - 1] == null)
        {
            _killsTemplates.Clear();
        }
        
        _canvasGroup.alpha = _killsTemplates.Count == 0f ? 0f : 1f;
    }

    public void InstantiateKills(string killerName, string deadPlayerName)
    {
        _photonView.RPC(nameof(InstantiateKillsRPC), RpcTarget.All, 
            killerName, deadPlayerName);
    }
    
    [PunRPC]
    private void InstantiateKillsRPC(string killerName, string deadPlayerName)
    {
        GameObject item = Instantiate(_killTemplate.gameObject, 
            _fartherPanel.position, Quaternion.identity, _fartherPanel);
        var killsItem = item.GetComponent<KillsItem>();
        _killsTemplates.Add(killsItem);
        killsItem.Initialize(killerName, deadPlayerName, _hideDelay);
    }
}
