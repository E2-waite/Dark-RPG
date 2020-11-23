using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enums;
public class Door : MonoBehaviour
{
    public DIR dir = DIR.N;
    public bool open = false;

    private void Start()
    {
        if (!open)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("DOOR TRIGGER");
            RoomController.Instance.ChangeRoom(dir);
        }
    }
}
