using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enums;
public class RoomController : MonoSingleton<RoomController>
{
    public List<GameObject> roomPrefabs;
    public GameObject player;
    Vector2Int mapSize;
    public GameObject[,] rooms;
    public GameObject currentRoom;
    public Vector2Int currentPos;
    bool changingRoom = false;
    private void Start()
    {
        SetupMap(new Vector2Int(10, 10));
    }

    public void SetupMap(Vector2Int size)
    {
        mapSize = size;
        rooms = new GameObject[mapSize.x, mapSize.y];
        bool[,] maze = GetComponent<GenerateMaze>().Generate(size);

        //Spawns rooms
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (maze[x,y])
                {
                    rooms[x, y] = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Count - 1)], new Vector3(0, 0, 0), Quaternion.identity);
                    rooms[x, y].transform.name = "Room " + x.ToString() + " " + y.ToString();
                    rooms[x, y].GetComponent<Room>().pos = new Vector2Int(x, y);
                    rooms[x, y].SetActive(false);
                }
            }
        }

        // Sets doors to open if there is a room in that direction
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (rooms[x, y] != null && RoomExists(new Vector2Int(x, y), (DIR)i))
                    {
                        rooms[x, y].GetComponent<Room>().doors[i].GetComponent<Door>().open = true;
                    }
                }
            }
        }

        StartCoroutine(SetStartRoom(size));
    }

    IEnumerator SetStartRoom(Vector2Int size)
    {
        //Randomly sets the room that the player starts in
        bool RoomSelected = false;
        while (!RoomSelected)
        {
            Vector2Int pos = new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
            if (rooms[pos.x, pos.y] != null)
            {
                rooms[pos.x, pos.y].SetActive(true);
                rooms[pos.x, pos.y].GetComponent<Room>().PlayerToEntrance(DIR.C, player);
                currentRoom = rooms[pos.x, pos.y];
                RoomSelected = true;
                Map.Instance.CreateMap(pos, size);
            }
            yield return null;
        }


    }

    bool RoomExists(Vector2Int pos, DIR dir)
    {
        if (dir == DIR.N && pos.x >= 0 && pos.y - 1 >= 0 &&  pos.x < rooms.GetLength(0) && pos.y - 1 < rooms.GetLength(1) && rooms[pos.x, pos.y - 1] != null)
        {
            return true;
        }
        else if (dir == DIR.E && pos.x + 1 >= 0 && pos.y >= 0 && pos.x + 1 < rooms.GetLength(0) && pos.y < rooms.GetLength(1) && rooms[pos.x + 1, pos.y] != null)
        {
            return true;
        }
        else if (dir == DIR.S && pos.x >= 0 && pos.y + 1 >= 0 && pos.x < rooms.GetLength(0) && pos.y + 1 < rooms.GetLength(1) && rooms[pos.x, pos.y + 1] != null)
        {
            return true;
        }
        else if (dir == DIR.W && pos.x - 1 >= 0 && pos.y >= 0 && pos.x - 1 < rooms.GetLength(0) && pos.y < rooms.GetLength(1) && rooms[pos.x - 1, pos.y] != null)
        {
            return true;
        }
        return false;
    }

    public bool ChangeRoom(DIR dir)
    {
        Vector2Int roomPos = currentRoom.GetComponent<Room>().pos;

        if (RoomExists(roomPos, dir))
        {
            currentRoom.SetActive(false);
            Vector2Int newPos = roomPos;
            if (dir == DIR.N)
            {
                newPos = new Vector2Int(roomPos.x, roomPos.y - 1);
            }
            else if (dir == DIR.E)
            {
                newPos = new Vector2Int(roomPos.x + 1, roomPos.y);
            }
            else if (dir == DIR.S)
            {
                newPos = new Vector2Int(roomPos.x, roomPos.y + 1);
            }
            else if (dir == DIR.W)
            {
                newPos = new Vector2Int(roomPos.x - 1, roomPos.y);
            }

            GameObject newRoom = rooms[newPos.x, newPos.y];
            currentPos = newPos;
            Map.Instance.MoveMap(dir);
            if (newRoom.GetComponent<Room>().PlayerToEntrance(dir, player))
            {
                newRoom.SetActive(true);
                currentRoom = newRoom;
            }
            return true;
        }
        else
        {
            Debug.Log("NO DOOR");
            changingRoom = false;
            return false;
        }
    }

    public void DeleteLevel()
    {
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Destroy(rooms[x, y]);
            }
        }
    }
}
