using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Generator : MonoBehaviour
{
    #region Variables
    [SerializeField] private int seed = 1;
    private List<GameObject> spawners = null;
    [SerializeField] private List<GameObject> rooms;
    [SerializeField] private Collider2D colliderInRadius;
    [Header("Gizmos")]
    [SerializeField] private Color32 color;
    #endregion

    Vector3 position;
    Vector2 size;

    // Use this for initialization
    void Start()
    {
        Random.InitState(seed);
        spawners = GameObject.FindGameObjectsWithTag("Spawner").ToList();
        StartCoroutine(Generate(5));
    }

    private IEnumerator Generate(int cycle)
    {
        for(int i = 0; i < cycle; i++)
        {
            spawners.Clear();
            spawners = GameObject.FindGameObjectsWithTag("Spawner").ToList();
            
            foreach (GameObject spawner in spawners)
            {
                GameObject room = rooms[Random.Range(0, rooms.Count)];
                RoomSpecs specs = room.GetComponent<RoomSpecs>();

                Vector2 offset = specs.Offset;
                float x = spawner.transform.position.x;
                float y = spawner.transform.position.y;
                float z = spawner.transform.position.z;

                position = new Vector3(x + offset.x, y + offset.y, z);
                size = specs.Size;

                colliderInRadius = Physics2D.OverlapBox(position, size, room.transform.rotation.z);
                if (colliderInRadius == null)
                    Instantiate(room, spawner.transform.position, spawner.transform.rotation);
                else
                {
                    Debug.Log("place wall");
                    Destroy(spawner);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(position, new Vector3(size.x, size.y, 0));
    }
}
