using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float xDist, yDist;
    public float xThreshold = 3, yThreshold = 5;
    public float followSpeed = 5;
    public bool follow = true;
    private void Update()
    {
        if (follow)
        {
            xDist = transform.position.x - player.transform.position.x;
            yDist = transform.position.y - player.transform.position.y;


            transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y, transform.position.z), Time.deltaTime * followSpeed);
            //transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }

    }
}
