using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Gun _currentGun;

    private Gun _pickedUpGun;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _currentGun.Shoot(_camera);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _currentGun.Reload();
        }
    }

    public void SetNewGun(Gun newGun)
    {
        _currentGun = newGun;
    }
}
