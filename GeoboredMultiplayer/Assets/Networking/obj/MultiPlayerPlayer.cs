using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerPlayer : MonoBehaviour {

    private NetworkManager netWorkManager;
    private string socketId;
    private bool mainPlayer;
    [SerializeField] private bool mainPlayerTest = false;

    public void Init(NetworkManager netWorkManager, string socketId, bool mainPlayer) {
        this.netWorkManager = netWorkManager;
        this.socketId = socketId;
        this.mainPlayer = mainPlayer;
    }

    /* Getters & Setters */
    public string GetSocketId() {
        return socketId;
    }

    public bool GetIfMainPlayer() {
        if (mainPlayerTest)
            return true;
        else if(netWorkManager == null)
            return true;
        return mainPlayer;
    }

    public NetworkManager GetNetworkManager() {
        return netWorkManager;
    }
}
