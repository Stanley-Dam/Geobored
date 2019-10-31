﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    // Start is called before the first frame update
    private void Start()
    {
        bullet = (GameObject)Resources.Load("Bullets/Prefabs/PistolBullet");
    }


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Fire();
    }
}