using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFidPanel : MonoBehaviour
{
    [SerializeField] private KillsItem _killTemplate;
    [SerializeField] private GameObject _fartherPanel;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private CanvasGroup _canvasGroup;

    private List<KillsItem> _killsTemplates = new List<KillsItem>();

    public void InstantiateKills(string killerName, string deadPlayerName)
    {
        GameObject killItem = PhotonNetwork.Instantiate(_killTemplate.name, _fartherPanel.transform.position, Quaternion.identity);
        var item = killItem.GetComponent<KillsItem>();
        _killsTemplates.Add(item);
        item.InstantiateKills(killerName, deadPlayerName);
        StartCoroutine(DisableWithDelay());
    }

    private IEnumerator DisableWithDelay()
    {
        yield return new WaitForSeconds(3f);

        _photonView.RPC(nameof(DisableRPC), RpcTarget.All);
    }

    [PunRPC]
    private void DisableRPC()
    {
        foreach (var kill in _killsTemplates)
        {
            Destroy(kill.gameObject);
        }

        _killsTemplates.Clear();
        _canvasGroup.alpha = 0f;
    }
}
