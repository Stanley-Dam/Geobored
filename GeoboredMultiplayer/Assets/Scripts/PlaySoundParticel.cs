using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundParticel : MonoBehaviour
{
    private AudioSource audioS;
    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        var main = this.GetComponent<ParticleSystem>().main;
        main.startColor = color;
        audioS = this.GetComponent<AudioSource>();
        float pitch = Random.Range(0.9f, 1.1f);
        audioS.pitch = pitch;
        audioS.Play(0);
    }

    public Color SetColor { set { this.color = value; } }
}
