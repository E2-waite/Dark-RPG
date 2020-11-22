using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enums;
public class Room : MonoBehaviour
{
    public Vector2Int pos;
    List<GameObject> enemies = new List<GameObject>();
    public GameObject[] doors = new GameObject[4];
    public bool doorsOpen = false;
    bool[] roomDirs = new bool[4];
    public Transform startPos;
    public Transform[] entrancePos = new Transform[4];

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count == 0 && !doorsOpen)
        {
            doorsOpen = true;
        }
    }

    public bool PlayerToEntrance(DIR dir, GameObject player)
    {
        player.transform.parent = null;
        player.transform.parent = transform;
        if (dir == DIR.N)
        {
            player.transform.position = entrancePos[(int)DIR.S].position;
        }
        else if (dir == DIR.E)
        {
            player.transform.position = entrancePos[(int)DIR.W].position;
        }
        else if (dir == DIR.S)
        {
            player.transform.position = entrancePos[(int)DIR.N].position;
        }
        else if (dir == DIR.W)
        {
            player.transform.position = entrancePos[(int)DIR.E].position;
        }
        else if (dir == DIR.C)
        {
            player.transform.position = startPos.position;
        }
        return true;
    }
}
