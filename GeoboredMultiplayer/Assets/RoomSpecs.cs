using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpecs : MonoBehaviour
{
    [SerializeField] private Vector2 size;
    [SerializeField] private Vector2 offset;
    private Vector3 position;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Position, new Vector3(size.x, size.y, 0));
    }

    public Vector3 Position
    {
        get
        {
            float x = transform.position.x;
            float y = transform.position.y;
            float z = transform.position.z;
            position = new Vector3(x + offset.x, y + offset.y, z);
            return position;
        }
    }
    public Vector2 Size { get { return size; } }
    public Vector2 Offset { get { return offset; } }
}