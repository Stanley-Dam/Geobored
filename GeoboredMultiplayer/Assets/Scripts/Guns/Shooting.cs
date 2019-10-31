using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
    #region Variables
    private GameObject pistolBullet;
    private Transform bulletSpawner;
    #endregion

    private void Start()
    {
        pistolBullet = (GameObject)Resources.Load("Bullets/Prefabs/PistolBullet");
        bulletSpawner = this.transform.Find("BulletSpawner");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Fire();
    }

    private void Fire()
    {
        Instantiate(pistolBullet, bulletSpawner.position, bulletSpawner.rotation);
    }
}
