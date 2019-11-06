using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    [SerializeField] private SocketIO.SocketIOComponent socket;
<<<<<<< Updated upstream
    [SerializeField] private GameObject playerPrefab;
=======
    [SerializeField] private CameraMovement cam;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject bulletPrefab;
>>>>>>> Stashed changes

    private ArrayList bullets = new ArrayList();
    private ArrayList players = new ArrayList();
    private bool joined = false;

    // Start is called before the first frame update
    void Start() {
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
        //Start listening to all the server events
        socket.On("connect", PlayerJoinServer);
        socket.On("move", MovePlayer);
        socket.On("join", PlayerJoin);
        socket.On("quit", PlayerQuit);
<<<<<<< Updated upstream
=======
        socket.On("bullet_spawn", BulletSpawn);
        socket.On("connect", PlayerJoinServer);
        socket.On("bullet_hit", BulletHitReceive);
>>>>>>> Stashed changes
    }

    /* Send data to the server. */

    private void PlayerJoinServer(SocketIO.SocketIOEvent e) {
        if(!joined) {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["x"] = "" + this.transform.position.x;
            data["y"] = "" + this.transform.position.y;
            data["rotZ"] = "" + this.transform.rotation.z;
            data["rotY"] = "" + this.transform.rotation.y;

            socket.Emit("join", new JSONObject(data));
        }
    }

<<<<<<< Updated upstream
    public void MovePlayerOnServer(Player player, Vector3 posTo) {
=======
    public void SpawnBulletOnServer(Vector3 position, Quaternion rotation) {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["clientId"] = "" + this.socket.sid;
        data["bulletUUID"] = "" + System.Guid.NewGuid();
        data["x"] = "" + position.x;
        data["y"] = "" + position.y;
        data["rotY"] = "" + rotation.eulerAngles.y;
        data["rotZ"] = "" + rotation.eulerAngles.z;

        socket.Emit("bullet_spawn", new JSONObject(data));
    }

    public void BulletHitOnServer(MultiPlayerBullet bullet) {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["clientId"] = "" + bullet.GetSocketId();
        data["bulletUUID"] = bullet.GetBulletUUID();

        socket.Emit("bullet_hit", new JSONObject(data));
    }

    public void MovePlayerOnServer(Player player, Vector3 posTo, Quaternion rotation) {
>>>>>>> Stashed changes
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["x"] = "" + posTo.x;
        data["y"] = "" + posTo.y;
        data["clientId"] = player.GetSocketId();

        this.transform.position = posTo;
        socket.Emit("move", new JSONObject(data));
    }

    /* Receive data from the server */

    private void MovePlayer(SocketIO.SocketIOEvent e) {
        string eventAsString = "" + e.data;
        Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventAsString);

        foreach(Player player in players) {
            if(player.GetSocketId() == data["clientId"])
                player.transform.position = new Vector3(float.Parse(data["x"]), float.Parse(data["y"]));
        }
    }

    /* Player 
     */

    /// <summary>
    /// deze funcite is voor je moeder
    /// </summary>
    /// <param name="e"></param>
    private void PlayerJoin(SocketIO.SocketIOEvent e) {
        string eventAsString = "" + e.data;
        Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventAsString);

<<<<<<< Updated upstream
        if(data["clientId"] == socket.sid) {
=======
        if(data["clientId"] == this.socket.sid &! joined) {
            joined = true;

>>>>>>> Stashed changes
            GameObject newPlayer = Instantiate(playerPrefab,
                new Vector3(float.Parse(data["x"]), float.Parse(data["y"])),
                Quaternion.Euler(float.Parse(data["rotX"]), float.Parse(data["rotY"]), 0));

            joined = true;
            Player player = newPlayer.GetComponent<Player>();
            player.Init(this, data["clientId"], true);
            players.Add(player);
<<<<<<< Updated upstream
        } else {
=======
            cam.SetPlayer(newPlayer);

        } else if(data["clientId"] != this.socket.sid) {
>>>>>>> Stashed changes
            GameObject newPlayer = Instantiate(playerPrefab,
                new Vector3(float.Parse(data["x"]), float.Parse(data["y"])),
                Quaternion.Euler(float.Parse(data["rotX"]), float.Parse(data["rotY"]), 0));

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

    /* Bullets
     */

    private void BulletHitReceive(SocketIO.SocketIOEvent e) {
        string eventAsString = "" + e.data;
        Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventAsString);

        foreach(MultiPlayerBullet bullet in bullets) {
            if(bullet.GetSocketId() == data["clientId"]) {
                bullets.Remove(bullet);
                Destroy(bullet.gameObject);
            }
        }
    }

    private void BulletSpawn(SocketIO.SocketIOEvent e) {
        string eventAsString = "" + e.data;
        Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventAsString);

        GameObject newBullet = Instantiate(bulletPrefab,
        new Vector3(float.Parse(data["x"]), float.Parse(data["y"])),
        Quaternion.Euler(0, float.Parse(data["rotY"]), float.Parse(data["rotZ"])));

        MultiPlayerBullet bullet = newBullet.GetComponent<MultiPlayerBullet>();
        bullet.Init(this, data["clientId"], data["clientId"] == this.socket.sid, data["bulletUUID"]);
        bullets.Add(bullet);
    }
}
