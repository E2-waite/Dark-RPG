using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoSingleton<RoomController>
{
    struct Room
    {
        public GameObject roomObj;
        public GameObject[] neighbours;
    }
    public GameObject[] roomObjs;
    public int xSize, ySize;
    Room[,] rooms;

    // Start is called before the first frame update
    void Start()
    {
        //rooms = new Room[xSize, ySize];
        //int num = 0;
        //for (int y = 0; y < ySize; y++)
        //{
        //    for (int x = 0; x < xSize; x++)
        //    {
        //        rooms[x, y] = new Room
        //        {
        //            roomObj = room1,
        //            neighbours{ }
        //        }
        //        num++;
        //}
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
