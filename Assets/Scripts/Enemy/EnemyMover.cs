using Enemy;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Stalker))]
public class EnemyMover : MonoBehaviour
{
    private Stalker _stalker;

    [SerializeField] private float _speed;

    public event UnityAction OnDie;
    
    private void Awake()
    {
        _stalker = GetComponent<Stalker>();
    }

    private void Update()
    {
        if (_stalker.TrySelectTarget() == false
            && _stalker.HasTarget == false)
            return;

        transform.position = Vector3.MoveTowards(transform.position,
            _stalker.Position, 
            _speed * Time.deltaTime);
        
        if (_stalker.GetDistance() < 0.2f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (GetComponent<PhotonView>().IsMine == false)
            return;
        
        _stalker.Contact();
        OnDie?.Invoke();
        PhotonNetwork.Destroy(gameObject);
    }
}
