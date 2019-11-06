using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    [SerializeField] private SocketIO.SocketIOComponent socket;
    [SerializeField] private CameraMovement cam;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject bulletPrefab;

    private ArrayList bullets = new ArrayList();
    private ArrayList players = new ArrayList();
    private bool joined = false;

    // Start is called before the first frame update
    void Start() {

        //Start listening to all the server events
        socket.On("move", MovePlayer);
        socket.On("join", PlayerJoin);
        socket.On("quit", PlayerQuit);
        socket.On("bullet_spawn", BulletSpawn);
        socket.On("connect", PlayerJoinServer);
        socket.On("bullet_hit", BulletHitReceive);
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

    /* Player 
     */

    /// <summary>
    /// deze funcite is voor je moeder
    /// </summary>
    /// <param name="e"></param>
    private void PlayerJoin(SocketIO.SocketIOEvent e) {
        string eventAsString = "" + e.data;
        Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(eventAsString);

        if(data["clientId"] == this.socket.sid &! joined) {
            joined = true;

            GameObject newPlayer = Instantiate(playerPrefab,
                new Vector3(float.Parse(data["x"]), float.Parse(data["y"])),
                Quaternion.Euler(0, float.Parse(data["rotY"]), float.Parse(data["rotZ"])));

            Player player = newPlayer.GetComponent<Player>();
            player.Init(this, data["clientId"], true);
            players.Add(player);
            cam.SetPlayer(newPlayer);

        } else if(data["clientId"] != this.socket.sid) {
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
