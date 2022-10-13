using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoBehaviour
    {
        private Rigidbody _rb;
        private Vector2 _moveDirection;

        [SerializeField] private float _runSpeed = 150f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var deltaSpeed = _runSpeed * Time.deltaTime;
            _rb.velocity = new Vector3(
                _moveDirection.x * deltaSpeed,
                0f,
                _moveDirection.y * deltaSpeed);   
        }
        
        public void Move(Vector2 direction)
        {
            _moveDirection = direction;
        }
}
