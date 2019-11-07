using UnityEngine;
using System.Collections;

public class AssaultRifel : Gun
{
    // Start is called before the first frame update
    private void Awake()
    {
        coolDownTime = 0.15f;
        bullet = (GameObject)Resources.Load("Bullets/Prefabs/PistolBullet");
        gunIcon = (GameObject)Resources.Load("WeaponIcons/GunIconAssaultRifel");
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(0) && canShoot)
            StartCoroutine(Fire(1));
    }
}
