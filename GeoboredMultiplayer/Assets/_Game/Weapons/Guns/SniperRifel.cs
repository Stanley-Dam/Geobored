using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifel : Gun
{
    private LineRenderer lr;
    //private Transform bulletSpawner;
    //private bool canShoot = true;
    private GameObject hitFX;

    // Start is called before the first frame update
    private void Start()
    {
        lr = this.GetComponent<LineRenderer>();
        bulletSpawner = this.transform.Find("BulletSpawner");
        hitFX = (GameObject)Resources.Load("Bullets/FX/hitFX");
    }

    // Update is called once per frame
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(bulletSpawner.position, bulletSpawner.right);
        lr.SetPosition(0, this.transform.position);
        lr.SetPosition(1, hit.point);
        lr.enabled = true;

        if (Input.GetMouseButton(0))
        {
            StartCoroutine(Fire());
        }
    } 

    private IEnumerator Fire()
    {
        Debug.DrawRay(bulletSpawner.position, bulletSpawner.right* 10f, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(bulletSpawner.position, bulletSpawner.right);
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
        Vector3 pos = hit.point;

        if (hitFX)
        {
            var hitFXIns = Instantiate(hitFX, pos, rot);
            try
            {
                Color color = hit.transform.GetComponent<SpriteRenderer>().color;
                hitFXIns.GetComponent<PlaySoundParticel>().SetColor = color;
            }
            catch
            {
                hitFXIns.GetComponent<PlaySoundParticel>().SetColor = Color.white;
            }

        }
        yield return null;
    }
}
