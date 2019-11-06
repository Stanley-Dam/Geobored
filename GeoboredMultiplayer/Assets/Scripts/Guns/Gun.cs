using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    protected GameObject bullet;
    protected int damage;
    protected Transform bulletSpawner;
    protected float spread = 4f;
    protected bool canShoot = true;
    protected float coolDownTime = 0.01f;

    protected void Awake()
    {
        bulletSpawner = this.transform.Find("BulletSpawner");
    }

    // Update is called once per frame
    protected IEnumerator Fire(int bulletAmount)
    {
        for(int i = 0; i < bulletAmount; i++)
        {
            bulletSpawner.localRotation = Quaternion.AngleAxis(Random.Range(-spread, spread), bulletSpawner.forward);
            var bulletIns = Instantiate(bullet, bulletSpawner.position, bulletSpawner.rotation);
            bulletIns.GetComponent<Bullet>().BulletOwner = this.gameObject;
            bulletSpawner.localRotation = Quaternion.AngleAxis(0, bulletSpawner.forward);
        }
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
