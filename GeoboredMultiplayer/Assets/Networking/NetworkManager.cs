using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    [SerializeField] private SocketIO.SocketIOComponent socket;

    private ArrayList players = new ArrayList();

    // Start is called before the first frame update
    void Start() {
        //Start listening to all the server events
        socket.On("move", MovePlayer);
        socket.On("join", PlayerJoin);

        //Now let's tell the server we've joined the party!
        socket.Emit("join");
    }

    /* Send data to the server. */

    public void MovePlayerOnServer(Vector3 posTo) {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["x"] = "" + posTo.x;
        data["y"] = "" + posTo.y;
        data["socket"] = socket.sid;

        this.transform.position = posTo;
        socket.Emit("move", new JSONObject(data));
    }

    /* Receive data from the server */

    private void MovePlayer(SocketIO.SocketIOEvent e) {
        string eventAsString = "" + e.data;
        Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventAsString);

        this.transform.position = new Vector3(float.Parse(data["x"]), float.Parse(data["y"]));
    }

    private void PlayerJoin(SocketIO.SocketIOEvent e) {
        string eventAsString = "" + e.data;
        Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventAsString);

        print(data);
    }

    private void PlayerQuit(SocketIO.SocketIOEvent e) {

    }
}
