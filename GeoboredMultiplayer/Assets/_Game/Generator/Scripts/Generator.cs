using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Generator : MonoBehaviour
{
    [SerializeField] private int seed = 1;
    private List<GameObject> spawners;

    // Use this for initialization
    void Start()
    {
        Random.InitState(seed);
        spawners = GameObject.FindGameObjectsWithTag("Spawner").ToList();
        foreach(GameObject spawner in spawners)
        {
            Debug.Log(spawner.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
