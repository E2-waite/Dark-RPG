using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using enums;
public class Map : MonoSingleton<Map>
{
    public Color grey, white;
    public GameObject nodePrefab;
    GameObject[,] nodes;
    GameObject currentNode;
    Vector2Int currentPos;
    public void CreateMap(Vector2Int startPos, Vector2Int size)
    {
        // Generates the map based on room grid
        currentPos = startPos;
        nodes = new GameObject[size.x, size.y];
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (RoomController.Instance.rooms[x,y] != null)
                {
                    nodes[x, y] = Instantiate(nodePrefab, transform);
                    nodes[x, y].GetComponent<RectTransform>().localPosition = new Vector3(x * 15, y * -15, 0);
                    nodes[x, y].GetComponent<Image>().color = grey;
                    nodes[x, y].SetActive(false);
                }
            }
        }

        CentreMap(size);
    }

    void CentreMap(Vector2Int size)
    {
        // Moves all tiles in the map to centre the current room
        currentNode = nodes[currentPos.x, currentPos.y];
        currentNode.GetComponent<Image>().color = white;
        currentNode.SetActive(true);
        int xDir = 15, yDir = 15;
        if (currentNode.GetComponent<RectTransform>().localPosition.x > 0)
        {
            xDir = -15;
        }
        if (currentNode.GetComponent<RectTransform>().localPosition.y > 0)
        {
            yDir = -15;
        }
        while (currentNode.GetComponent<RectTransform>().localPosition.x != 0)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    if (nodes[x, y] != null)
                    {
                        nodes[x, y].GetComponent<RectTransform>().localPosition = new Vector3(nodes[x, y].GetComponent<RectTransform>().localPosition.x + xDir, 
                            nodes[x, y].GetComponent<RectTransform>().localPosition.y, 0);
                    }               
                }
            }
        }
        while (currentNode.GetComponent<RectTransform>().localPosition.y != 0)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    if (nodes[x, y] != null)
                    {
                        nodes[x, y].GetComponent<RectTransform>().localPosition = new Vector3(nodes[x, y].GetComponent<RectTransform>().localPosition.x,
                            nodes[x, y].GetComponent<RectTransform>().localPosition.y + yDir, 0);
                    }
                }
            }
        }
    }

    public void MoveMap(DIR dir)
    {
        // Moves the map to centre the current room and change room colours;
        nodes[currentPos.x, currentPos.y].GetComponent<Image>().color = grey;
        Vector2Int dirVec = new Vector2Int(0,0);
        if (dir == DIR.N)
        {
            dirVec = new Vector2Int(0, -15);
            nodes[currentPos.x, currentPos.y - 1].GetComponent<Image>().color = white;
            nodes[currentPos.x, currentPos.y - 1].SetActive(true);
            currentPos = new Vector2Int(currentPos.x, currentPos.y - 1);
        }
        if (dir == DIR.E)
        {
            dirVec = new Vector2Int(-15, 0);
            nodes[currentPos.x + 1, currentPos.y].GetComponent<Image>().color = white;
            nodes[currentPos.x + 1, currentPos.y].SetActive(true);
            currentPos = new Vector2Int(currentPos.x + 1, currentPos.y);
        }
        if (dir == DIR.S)
        {
            dirVec = new Vector2Int(0, 15);
            nodes[currentPos.x, currentPos.y + 1].GetComponent<Image>().color = white;
            nodes[currentPos.x, currentPos.y + 1].SetActive(true);
            currentPos = new Vector2Int(currentPos.x, currentPos.y + 1);
        }
        if (dir == DIR.W)
        {
            dirVec = new Vector2Int(15, 0);
            nodes[currentPos.x - 1, currentPos.y].GetComponent<Image>().color = white;
            nodes[currentPos.x - 1, currentPos.y].SetActive(true);
            currentPos = new Vector2Int(currentPos.x - 1, currentPos.y);
        }

        for (int y = 0; y < nodes.GetLength(1); y++)
        {
            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                if (nodes[x, y] != null)
                {
                    nodes[x, y].GetComponent<RectTransform>().localPosition = nodes[x, y].GetComponent<RectTransform>().localPosition + (Vector3)(Vector3Int)dirVec;

                }
            }
        }
    }
}
