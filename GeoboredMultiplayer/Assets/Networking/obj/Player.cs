using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private NetworkManager netWorkManager;
    private string socketId;
    private bool mainPlayer;

    public void Init(NetworkManager netWorkManager, string socketId, bool mainPlayer) {
        this.netWorkManager = netWorkManager;
        this.socketId = socketId;
        this.mainPlayer = mainPlayer;
    }


    /* TEMPORARY just here for testing :P */
    void Update() {
        if(mainPlayer) {
            Vector3 moveTo = new Vector3(this.transform.position.x, this.transform.position.y);

            if(Input.GetKey(KeyCode.A)) {
                moveTo += new Vector3(-10 * Time.deltaTime, 0);
            }

            if(Input.GetKey(KeyCode.D)) {
                moveTo += new Vector3(10 * Time.deltaTime, 0);
            }

            if(Input.GetKey(KeyCode.W)) {
                moveTo += new Vector3(0, 10 * Time.deltaTime);
            }

            if(Input.GetKey(KeyCode.S)) {
                moveTo += new Vector3(0, -10 * Time.deltaTime);
            }

            if(moveTo != transform.position)
                netWorkManager.MovePlayerOnServer(this, moveTo);

            this.transform.position = moveTo;
        }
    }

    /* Getters & Setters */
    public string GetSocketId() {
        return socketId;
    }
}
