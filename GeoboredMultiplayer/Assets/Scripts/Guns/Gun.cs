using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    protected GameObject bullet;
    protected int damage;
    protected Transform bulletSpawner;
    protected float spread;
    protected bool canShoot = true;
    protected float coolDownTime = 0.01f;

    protected void Awake()
    {
        bulletSpawner = this.transform.Find("BulletSpawner");
    }

    // Update is called once per frame
    protected IEnumerator Fire()
    {
        Instantiate(bullet, bulletSpawner.position, bulletSpawner.rotation);
        canShoot = false;
        StartCoroutine(CoolDown());
        yield return null;
        
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(coolDownTime);
        canShoot = true;
    }
}
