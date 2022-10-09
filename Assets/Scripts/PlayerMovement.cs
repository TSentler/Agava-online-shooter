using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPunObservable
{
    [SerializeField] private float _speed;
    [SerializeField] private PhotonView _photonViev;
    [SerializeField] private CharacterController _characterController;

    private const string RunAnimation = "IsRun";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_photonViev.IsMine)
        {
            var direction = new Vector3(
           Input.GetAxisRaw("Horizontal"),0f,
           Input.GetAxisRaw("Vertical"));

            if (direction != Vector3.zero)
            {
                _characterController.Move(direction * _speed * Time.deltaTime);
                _animator.SetBool(RunAnimation, true);
            }
            else
            {
                _animator.SetBool(RunAnimation, false);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
