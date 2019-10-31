using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    protected GameObject bullet;
    protected int damage;
    protected Transform bulletSpawner;
    protected float spread;

    protected void Awake()
    {
        bulletSpawner = this.transform.Find("BulletSpawner");
    }

    // Update is called once per frame
    protected void Fire()
    {
        Instantiate(bullet, bulletSpawner.position, bulletSpawner.rotation);
    }

}
