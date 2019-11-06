using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerBullet : MonoBehaviour {
    private NetworkManager netWorkManager;
    private string socketId;
    private string bulletUUID;
    private bool manageBullet;

    public void Init(NetworkManager netWorkManager, string socketId, bool manageBullet, string bulletUUID) {
        this.netWorkManager = netWorkManager;
        this.socketId = socketId;
        this.manageBullet = manageBullet;
        this.bulletUUID = bulletUUID;
    }

    /* Getters & Setters */
    public string GetSocketId() {
        return socketId;
    }

    public bool GetBulletManagedByClient() {
        return manageBullet;
    }

    public NetworkManager GetNetworkManager() {
        return netWorkManager;
    }

    public string GetBulletUUID() {
        return this.bulletUUID;
    }
}
