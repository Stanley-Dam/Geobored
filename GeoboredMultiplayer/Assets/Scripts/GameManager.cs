using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private List<GameObject> livingPlayers = new List<GameObject>();
    private UIHandler uiHandler;
    private TextMeshProUGUI winText;
    private GameObject activePlayer;

    private void Start()
    {
        Time.timeScale = 1f;
        livingPlayers = GameObject.FindGameObjectsWithTag("Player").ToList();
        foreach (GameObject player in livingPlayers)
            if (player.GetComponent<Player>().GetIfMainPlayer())
                this.activePlayer = player;
        uiHandler = GameObject.FindWithTag("Canvas").GetComponent<UIHandler>();
    }

    public void PlayerDied(GameObject deadPlayer)
    {
        livingPlayers.Remove(deadPlayer);
        if(livingPlayers.Count == 1)
        {
            StartCoroutine(uiHandler.SlowMotionWin());
        }
    }

    public List<GameObject> GetLivingPlayers { get { return this.livingPlayers; } }
    public GameObject ActivePlayer { get { return this.activePlayer; } }

}