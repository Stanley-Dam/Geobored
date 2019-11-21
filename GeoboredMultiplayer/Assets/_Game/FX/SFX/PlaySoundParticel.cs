using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundParticel : MonoBehaviour
{
    #region Variables
    private AudioSource audioS;
    private Color particelColor;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem.MainModule main = this.GetComponent<ParticleSystem>().main;
        main.startColor = particelColor;
        audioS = this.GetComponent<AudioSource>();
        float pitch = Random.Range(0.9f, 1.1f);
        audioS.pitch = pitch;
        audioS.Play(0);
    }

    //getters and setters
    public Color ParticelColor { set { particelColor = value; } }
}
