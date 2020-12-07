using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    public enum INTERACTION
    {
        chest,
        mimic,
        shop
    }

    bool active = true;
    public GameObject display;
    public INTERACTION type;

    public bool Interact(GameObject player)
    {
        if (type == INTERACTION.chest)
        {
            if (GetComponent<Chest>().Open())
            {
                display.SetActive(false);
                active = false;
                return true;
            }
        }
        if (type == INTERACTION.mimic)
        {
            if (GetComponent<Mimic>().Open())
            {
                display.SetActive(false);
                active = false;
                return true;
            }
        }
        if (type == INTERACTION.shop)
        {
            if (GetComponent<Shop>().Buy(player))
            {
                display.SetActive(false);
                active = false;
                return true;
            }
        }
        return false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCol")
        {
            if (active)
            {
                display.SetActive(true);
                collision.transform.parent.gameObject.GetComponent<PlayerController>().interaction = gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCol")
        {
            display.SetActive(false);
            collision.transform.parent.gameObject.GetComponent<PlayerController>().interaction = null;
        }
    }
}
