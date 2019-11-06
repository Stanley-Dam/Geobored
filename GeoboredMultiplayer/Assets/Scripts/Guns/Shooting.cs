using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
    #region Variables
    [SerializeField] private Player multiplayerPlayer;
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
        if(multiplayerPlayer != null && !multiplayerPlayer.GetIfMainPlayer())
            return;

        multiplayerPlayer.GetNetworkManager().SpawnBulletOnServer(bulletSpawner.position, bulletSpawner.rotation);
    }
}
