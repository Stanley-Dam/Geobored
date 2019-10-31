using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    [SerializeField] private SocketIO.SocketIOComponent socket;
    private GameObject playerPrefab;

    private ArrayList players = new ArrayList();
    private bool joined = false;

    // Start is called before the first frame update
    void Start() {
        playerPrefab = Resources.Load<GameObject>("Players/Player1");

        //Start listening to all the server events
        socket.On("move", MovePlayer);
        socket.On("join", PlayerJoin);
        socket.On("quit", PlayerQuit);
        socket.On("connect", PlayerJoinServer);
    }

    /* Send data to the server. */

    private void PlayerJoinServer(SocketIO.SocketIOEvent e) {
        if(!joined) {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["x"] = "" + this.transform.position.x;
            data["y"] = "" + this.transform.position.y;
            data["rotX"] = "" + this.transform.rotation.x;
            data["rotY"] = "" + this.transform.rotation.y;

            socket.Emit("join", new JSONObject(data));
        }
    }

    public void MovePlayerOnServer(Player player, Vector3 posTo, Quaternion rotation) {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["x"] = "" + posTo.x;
        data["y"] = "" + posTo.y;
        data["clientId"] = player.GetSocketId();
        data["rotZ"] = "" + rotation.eulerAngles.z;
        data["rotY"] = "" + rotation.eulerAngles.y;

        this.transform.position = posTo;
        socket.Emit("move", new JSONObject(data));
    }

    /* Receive data from the server */

    private void MovePlayer(SocketIO.SocketIOEvent e) {
        string eventAsString = "" + e.data;
        Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventAsString);
        
        foreach(Player player in players) {
            if(player.GetSocketId() == data["clientId"] &! player.GetIfMainPlayer()) {
                player.transform.position = new Vector3(float.Parse(data["x"]), float.Parse(data["y"]));
                player.transform.rotation = Quaternion.Euler(0, float.Parse(data["rotY"]), float.Parse(data["rotZ"]));
            }
        }
    }

    private void PlayerJoin(SocketIO.SocketIOEvent e) {
        string eventAsString = "" + e.data;
        Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventAsString);

        if(data["clientId"] == socket.sid &! joined) {
            joined = true;

            GameObject newPlayer = Instantiate(playerPrefab,
                new Vector3(float.Parse(data["x"]), float.Parse(data["y"])),
                Quaternion.Euler(0, float.Parse(data["rotY"]), float.Parse(data["rotZ"])));

            Player player = newPlayer.GetComponent<Player>();
            player.Init(this, data["clientId"], true);
            players.Add(player);
        } else if(data["clientId"] != socket.sid) {
            GameObject newPlayer = Instantiate(playerPrefab,
                new Vector3(float.Parse(data["x"]), float.Parse(data["y"])),
                Quaternion.Euler(0, float.Parse(data["rotY"]), float.Parse(data["rotZ"])));

            Player player = newPlayer.GetComponent<Player>();
            player.Init(this, data["clientId"], false);
            players.Add(player);
        }
            
    }

    private void PlayerQuit(SocketIO.SocketIOEvent e) {
        string eventAsString = "" + e.data;
        Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventAsString);

        foreach(Player player in players) {
            if(player.GetSocketId() == data["clientId"])
                Destroy(player.gameObject);
        }
    }
}
