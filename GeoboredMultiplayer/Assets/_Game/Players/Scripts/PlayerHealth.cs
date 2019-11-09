using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour
{
    #region Variables
    [SerializeField] private float health = 100;
    private GameObject healthBarHolder;
    private TextMeshProUGUI healthText;
    private Image primaryHealthBar, secondaryHealthBar;

    [Header("FX")]
    [SerializeField] private GameObject killFX;
    private Color playerColor;
    [SerializeField] private AudioSource audioS;
    [SerializeField] private string winMessage = "";

    [Header("Misc")]
    [SerializeField] private MultiPlayerPlayer MultiPlayerPlayer;
    private GameManager gameManager;
    private GameObject killer;
    private bool isMainPlayer = false;
    #endregion

    // Use this for initialization
    void Start()
    {
        if (MultiPlayerPlayer.GetIfMainPlayer())
            isMainPlayer = true;
        healthBarHolder = GameObject.FindWithTag("Health");
        healthText = healthBarHolder.transform.Find("HealthText").GetComponent<TextMeshProUGUI>();
        primaryHealthBar = healthBarHolder.transform.Find("PrimaryHealthBar").GetComponent<Image>();
        secondaryHealthBar = healthBarHolder.transform.Find("SecondaryHealthBar").GetComponent<Image>();
        playerColor = this.transform.Find("Sprite").GetComponent<SpriteRenderer>().color;
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            TakeDamage(20);
    }

    public void TakeDamage(int damage)
    {
        if (!audioS.enabled)
            audioS.enabled = true;
        //plays sound fx with random pitch
        float pitch = Random.Range(0.9f, 1.1f);
        audioS.pitch = pitch;
        audioS.Play(0);

        health -= damage;
        if(MultiPlayerPlayer.GetIfMainPlayer())
        {
            healthText.text = $"{Mathf.Floor(health)}";
            primaryHealthBar.fillAmount = health / 100;
            StartCoroutine(HealthBarEffect());
        }
        if (health <= 0)
            KillPlayer();
    }

    private void KillPlayer()
    {
        if(playerColor == null)
        {
            List<GameObject> players = gameManager.LivingPlayers;
            killer = players[Random.Range(0, players.Count)];
        }
        Camera.main.GetComponent<CameraMovement>().SetPlayer(killer);
        killer.GetComponentInParent<PlayerHealth>().SetIsMainPlayer(true);
        GameObject killFXIns = Instantiate(killFX, this.transform.position, killFX.transform.rotation);
        killFXIns.GetComponent<PlaySoundParticel>().ParticelColor = playerColor;
        gameManager.PlayerDied(this.gameObject);
        Destroy(this.gameObject);
    }

    private IEnumerator HealthBarEffect()
    {
        while (primaryHealthBar.fillAmount != secondaryHealthBar.fillAmount)
        {
            secondaryHealthBar.fillAmount = Mathf.Lerp(secondaryHealthBar.fillAmount, primaryHealthBar.fillAmount, 2f * Time.deltaTime);
            yield return null;
        }
    }

    //getters and setters
    public string WinMessage { get { return winMessage; } }
    public Color PlayerColor { get { return playerColor; } }
    public GameObject Killer { set { killer = value; } }
    public void SetIsMainPlayer(bool isMainPlayer)
    {
        this.isMainPlayer = isMainPlayer;
        TakeDamage(0);
    }
}
