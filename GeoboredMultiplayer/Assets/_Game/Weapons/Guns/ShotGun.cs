using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Gun
{
    // Start is called before the first frame update
    private void Awake()
    {
        spread = 7f;
        bullet = (GameObject)Resources.Load("Bullets/Prefabs/ShotGunShell");
    }


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            StartCoroutine(Fire(5));
        }   
    }
}
