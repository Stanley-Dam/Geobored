using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    #region Variables
    private List<GameObject> livingPlayers;
    private UIHandler uiHandler;
    private TextMeshProUGUI winText;
    private Camera cam;
    #endregion

    private void Start()
    {
        Time.timeScale = 1f;
        livingPlayers = GameObject.FindGameObjectsWithTag("Player").ToList();
        uiHandler = GameObject.FindWithTag("Canvas").GetComponent<UIHandler>();
        cam = Camera.main;
    }

    public void PlayerDied(GameObject deadPlayer)
    {
        livingPlayers.Remove(deadPlayer);
        if(livingPlayers.Count == 1)
        {
            cam.GetComponent<CameraMovement>().SetPlayer(livingPlayers[0]);
            StartCoroutine(uiHandler.SlowMotion());
        }
    }

    public List<GameObject> LivingPlayers { get { return livingPlayers; } }

}