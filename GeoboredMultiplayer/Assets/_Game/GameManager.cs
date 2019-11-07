using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private List<GameObject> livingPlayers;
    private UIHandler uiHandler;
    private TextMeshProUGUI winText;

    private void Start()
    {
        Time.timeScale = 1f;
        livingPlayers = GameObject.FindGameObjectsWithTag("Player").ToList();
        uiHandler = GameObject.FindWithTag("Canvas").GetComponent<UIHandler>();
    }

    public void PlayerDied(GameObject deadPlayer)
    {
        livingPlayers.Remove(deadPlayer);
        if(livingPlayers.Count == 1)
        {
            StartCoroutine(uiHandler.SlowMotion());
        }
    }

    public List<GameObject> GetLivingPlayers { get { return this.livingPlayers; } }

}