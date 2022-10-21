using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.LookAt(
            transform.position + Camera.main.transform.forward);
    }
}
