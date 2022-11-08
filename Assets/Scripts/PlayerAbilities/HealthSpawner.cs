using Photon.Pun;
using System.Collections;
using UnityEngine;

public class HealthSpawner : ItemPresenter
{
    [SerializeField] private float _healValue;

    private void OnTriggerEnter(Collider other)
    {
        if (CanUse)
        {
            if (other.TryGetComponent(out PlayerAbilities.PlayerHealth playerHealth))
            {
                if (playerHealth.NeedHeal())
                {
                    playerHealth.TakeHeal(_healValue);
                    PhotonView.RPC(nameof(DisableItem), RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    private void DisableItem()
    {
        ShowPoint.gameObject.SetActive(false);
        CanUse = false;
        Collider.enabled = false;
        StartCoroutine(CountdownToSpawn());
    }

    private IEnumerator CountdownToSpawn()
    {
        yield return new WaitForSeconds(TimerToSpawn);
        ShowPoint.gameObject.SetActive(true);
        Collider.enabled = true;
        CanUse = true;
    }
}
