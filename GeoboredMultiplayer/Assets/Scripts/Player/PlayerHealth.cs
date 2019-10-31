using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    #region Variables
    private float health = 100;
    private GameObject haelth;
    private TextMeshProUGUI healthText;
    private Image primaryHealthBar, secondaryHealthBar;
    private GameObject killFX;
    private Color playerColor;
    private AudioSource audioS;
    private GameManager gameManager;
    private UIHandler uiHandler;
    private string winString;
    #endregion

    // Use this for initialization
    void Start()
    {
        haelth = GameObject.FindWithTag("Health");
        killFX = (GameObject)Resources.Load("Player/FX/KillPlayerFX");
        playerColor = this.transform.Find("Sprite").GetComponent<SpriteRenderer>().color;
        audioS = this.GetComponent<AudioSource>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        uiHandler = GameObject.FindWithTag("Canvas").GetComponent<UIHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            TakeDamage(20);
    }

    private void TakeDamage(int damage)
    {
        //plays sound fx with random pitch
        float pitch = Random.Range(0.9f, 1.1f);
        audioS.pitch = pitch;
        audioS.Play(0);

        health -= damage;
        uiHandler.UpdateHealthBar(this.gameObject, health);

        if (health <= 0)
            KillPlayer();
    }

    private void KillPlayer()
    {
        //haelth.SetActive(false);
        var killFXIns = Instantiate(killFX, this.transform.position, killFX.transform.rotation);
        killFXIns.GetComponent<PlaySoundParticel>().SetColor = playerColor;
        gameManager.PlayerDied(this.gameObject);
        Destroy(this.gameObject);
    }

    public string GetWinString { get { return this.winString; } }
}
