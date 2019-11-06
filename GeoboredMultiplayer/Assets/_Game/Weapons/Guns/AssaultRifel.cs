using UnityEngine;
using System.Collections;

public class AssaultRifel : Gun
{
    // Start is called before the first frame update
    private void Start()
    {
        coolDownTime = 0.1f;
        bullet = (GameObject)Resources.Load("Bullets/Prefabs/PistolBullet");
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButton(0) && canShoot)
            StartCoroutine(Fire(1));
    }
}
