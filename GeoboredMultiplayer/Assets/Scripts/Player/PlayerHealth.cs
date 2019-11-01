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
    private string winString;
    #endregion

    // Use this for initialization
    void Start()
    {
        haelth = GameObject.FindWithTag("Health");
        healthText = haelth.transform.Find("HealthText").GetComponent<TextMeshProUGUI>();
        primaryHealthBar = haelth.transform.Find("PrimaryHealthBar").GetComponent<Image>();
        secondaryHealthBar = haelth.transform.Find("SecondaryHealthBar").GetComponent<Image>();
        killFX = (GameObject)Resources.Load("Player/FX/KillPlayerFX");
        playerColor = this.transform.Find("Sprite").GetComponent<SpriteRenderer>().color;
        audioS = this.GetComponent<AudioSource>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
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
        healthText.text = $"{Mathf.Floor(health)}";
        primaryHealthBar.fillAmount = health / 100;
        if (health <= 0)
            KillPlayer();
        StartCoroutine(HealthBarEffect());
    }

    private void KillPlayer()
    {
        haelth.SetActive(false);
        var killFXIns = Instantiate(killFX, this.transform.position, killFX.transform.rotation);
        killFXIns.GetComponent<PlaySoundParticel>().SetColor = playerColor;
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

    public string GetWinString { get { return this.winString; } }
}
