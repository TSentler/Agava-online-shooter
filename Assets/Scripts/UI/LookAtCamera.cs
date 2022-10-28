using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        if (Camera.main == null)
            return;
        
        transform.LookAt(
            transform.position + Camera.main.transform.forward);
    }
}
