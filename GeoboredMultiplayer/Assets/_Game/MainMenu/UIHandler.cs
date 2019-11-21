using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameManager gameManager;
    private float slowDownFector = 0.05f;
    private float slowDownLenght = 2f;
    private TextMeshProUGUI winText;
    #endregion

    private void Start()
    {
        winText = GameObject.FindWithTag("Canvas").transform.Find("WinText").GetComponent<TextMeshProUGUI>();
    }

    public void Singleplayer()
    {
        SceneManager.LoadScene("Singleplayer");
    }
    public void Multiplayer()
    {
        SceneManager.LoadScene("Multiplayer");
    }
    public void Test()
    {   
        SceneManager.LoadScene("Test");
    }

    public IEnumerator SlowMotion()
    {
        Time.timeScale = slowDownFector;
        Time.fixedDeltaTime = Time.timeScale * 0.2f;
        yield return new WaitForSeconds(0.1f);
        string text = gameManager.LivingPlayers[0].GetComponent<PlayerHealth>().WinMessage;
        winText.color = gameManager.LivingPlayers[0].GetComponent<PlayerHealth>().PlayerColor;
        if (text != "" && text != null)
            winText.text = text;
        else
            winText.text = "Winner";
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}