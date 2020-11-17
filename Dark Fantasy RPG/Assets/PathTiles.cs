using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PathTiles : MonoBehaviour
{
    public Transform startPos, endPos;
    public float density = 10;
    public float numX, numY;

    private void Start()
    {
        float distX = endPos.position.x - startPos.position.x;
        float distY = endPos.position.y - startPos.position.y;

        numX = distX / density;
        numY = distY / density;
    }
}
