using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] private protected float DelayPerShoot;
    [SerializeField] private protected float DelayReload;
    [SerializeField] private protected int MaxAmmo;

    private protected int AmmoQuanity;
    private protected bool CanShoot = true;

    public abstract void Shoot(Camera camera);

    private void Start()
    {
        AmmoQuanity = MaxAmmo;
    }

    public void Reload()
    {
        if (AmmoQuanity < MaxAmmo)
            StartCoroutine(RestoreAmmo());
    }

    protected IEnumerator CountdownShoot()
    {
        CanShoot = false;
        yield return new WaitForSeconds(DelayPerShoot);
        CanShoot = true;
    }

    private IEnumerator RestoreAmmo()
    {
        CanShoot = false;
        yield return new WaitForSeconds(DelayReload);
        CanShoot = true;
        AmmoQuanity = MaxAmmo;
    }
}
