using Photon.Pun;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(PhotonView))]
public class HealthSpawner : MonoBehaviour
{
    [SerializeField] private float _healValue;
    [SerializeField] private Transform _showPoint;
    [SerializeField] private float _timerToSpawn;

    private PhotonView _photonView;
    private SphereCollider _collider;
    private bool _canUse = true;
    private Vector3 _rotation = new Vector3(0, 40, 0);

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_canUse)
        {
            if (other.TryGetComponent(out PlayerAbilities.PlayerHealth playerHealth))
            {
                if (playerHealth.NeedHeal())
                {
                    playerHealth.TakeHeal(_healValue);
                    _photonView.RPC(nameof(DisableItem), RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    private void DisableItem()
    {
        _showPoint.gameObject.SetActive(false);
        _canUse = false;
        _collider.enabled = false;
        StartCoroutine(CountdownToSpawn());
    }

    private IEnumerator CountdownToSpawn()
    {
        yield return new WaitForSeconds(_timerToSpawn);
        _showPoint.gameObject.SetActive(true);
        _collider.enabled = true;
        _canUse = true;
    }
}
