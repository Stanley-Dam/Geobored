using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunShell : Bullet
{
    private Rigidbody2D rb;
    // Start is called before the first frame update
    private void Update()
    {
        this.transform.position += transform.right * bulletSpeed * Time.deltaTime;
    }
}
