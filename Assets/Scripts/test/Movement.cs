using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class Movement : MonoBehaviour
{
    private PhotonView _view;
    
    [SerializeField] private float _speed = 6f;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (_view.IsMine == false)
            return;
        
        var direction = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"), 0f).normalized;
        transform.Translate(direction * _speed * Time.deltaTime);
    }
}
