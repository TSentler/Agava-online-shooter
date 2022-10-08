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
        //_photonViev = FindObjectOfType<PhotonView>();
    }

    private void Update()
    {
        if (_photonViev.IsMine)
        {
            float axisHorizontal = Input.GetAxis("Horizontal");
            float axisVertical = Input.GetAxis("Vertical");
            //float mouseX = Input.GetAxis("Mouse X");
            //float mouseY = Input.GetAxis("Mouse Y");

            if (axisHorizontal != 0 || axisVertical != 0)
            {
                Vector3 movementVector = transform.right * axisHorizontal + transform.forward * axisVertical;
                //transform.Translate(movementVector * _speed * Time.deltaTime);
                _characterController.Move(movementVector * _speed * Time.deltaTime);
                _animator.SetBool(RunAnimation, true);
                //gameObject.transform.LookAt(movementVector);
            }
            else
            {
                _animator.SetBool(RunAnimation, false);
            }

            //transform.rotation *= Quaternion.Euler(0, mouseX * _mouseSensitivity * Time.deltaTime, 0);
        }  
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
