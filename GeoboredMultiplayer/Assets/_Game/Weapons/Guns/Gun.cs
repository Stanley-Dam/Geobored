using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    #region Variables
    [Header("Gun")]
    [SerializeField] private bool singleFire = false;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int bulletAmount = 1;
    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float spread = 0f;
    [SerializeField] private float coolDownTime = 0.1f;
    private Transform bulletSpawner;
    private bool canShoot = true;
    private float coolDownTimeRemaining;

    [Header("Gun Icon")]
    [SerializeField] private GameObject gunIcon;
    private Image gunIconOverlay;

    private MultiPlayerPlayer multiPlayerPlayer;
    #endregion

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "BulletSpawner")
                bulletSpawner = child;
        }
        multiPlayerPlayer = GetComponentInParent<MultiPlayerPlayer>();
        if(multiPlayerPlayer.GetIfMainPlayer())
        {
            Transform gunIconLocation = GameObject.FindWithTag("Health").transform.Find("Gun Icon");
            GameObject iconIns = Instantiate(gunIcon, gunIconLocation);
            gunIconOverlay = iconIns.transform.Find("IconBackGround").GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot && singleFire)
            StartCoroutine(Fire(bulletAmount));
        if (Input.GetMouseButton(0) && canShoot && !singleFire)
            StartCoroutine(Fire(bulletAmount));
    }

    // Update is called once per frame
    private IEnumerator Fire(int bulletAmount)
    {
        for (int i = 0; i < bulletAmount; i++)
        {
            bulletSpawner.localRotation = Quaternion.AngleAxis(Random.Range(-spread, spread), bulletSpawner.forward);
            GameObject bulletIns = Instantiate(bullet, bulletSpawner.position, bulletSpawner.rotation);
            bulletIns.GetComponent<Bullet>().SetDamage = damage;
            bulletIns.GetComponent<Bullet>().SetBulletSpeed = bulletSpeed;
            bulletIns.GetComponent<Bullet>().BulletOwner = this.gameObject;
            bulletSpawner.localRotation = Quaternion.AngleAxis(0, bulletSpawner.forward);
        }
        canShoot = false;
        if(coolDownTime != 0)
            StartCoroutine(CoolDown());
        yield return null;
    }

    private IEnumerator CoolDown()
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
