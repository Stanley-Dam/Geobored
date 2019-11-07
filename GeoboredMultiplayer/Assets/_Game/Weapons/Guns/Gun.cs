using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    protected GameObject bullet;
    protected int damage;
    protected Transform bulletSpawner;
    protected float spread = 4f;
    protected bool canShoot = true;
    protected float coolDownTime = 0.1f;
    protected float coolDownTimeRemaining;
    protected Image gunIconOverlay;
    protected GameObject gunIcon;
    private MultiPlayerPlayer multiPlayerPlayer;

    protected void Start()
    {
        bulletSpawner = this.transform.Find("BulletSpawner");
        multiPlayerPlayer = this.GetComponent<MultiPlayerPlayer>();
        if(true /*multiPlayerPlayer.GetIfMainPlayer()*/)
        {
            Transform gunIconLocation = GameObject.FindWithTag("Health").transform.Find("Gun Icon");
            GameObject iconIns = Instantiate(gunIcon, gunIconLocation);
            gunIconOverlay = iconIns.GetComponentInChildren<Image>();
        }
    }

    // Update is called once per frame
    protected IEnumerator Fire(int bulletAmount)
    {
        for (int i = 0; i < bulletAmount; i++)
        {
            bulletSpawner.localRotation = Quaternion.AngleAxis(Random.Range(-spread, spread), bulletSpawner.forward);
            GameObject bulletIns = Instantiate(bullet, bulletSpawner.position, bulletSpawner.rotation);
            bulletIns.GetComponent<Bullet>().BulletOwner = this.gameObject;
            bulletSpawner.localRotation = Quaternion.AngleAxis(0, bulletSpawner.forward);
        }
        canShoot = false;
        StartCoroutine(CoolDown());
        yield return null;
    }

    IEnumerator CoolDown()
    {
        for (coolDownTimeRemaining = coolDownTime; coolDownTimeRemaining > 0; coolDownTimeRemaining -= Time.deltaTime)
        {
            yield return null;
            float fillAmount = 1 * coolDownTimeRemaining / coolDownTime;
            gunIconOverlay.fillAmount = fillAmount;
        }
        gunIconOverlay.fillAmount = 0;
        canShoot = true;
        
    }
}
