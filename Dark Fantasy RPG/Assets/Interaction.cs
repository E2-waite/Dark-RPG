using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    public enum INTERACTION
    {
        chest,
        shop
    }

    bool active = true;
    public GameObject E;
    public INTERACTION type;

    public bool Interact()
    {
        if (type == INTERACTION.chest)
        {
            if (GetComponent<Chest>().Open())
            {
                E.SetActive(false);
                active = false;
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (active)
            {
                E.SetActive(true);
                collision.gameObject.GetComponent<PlayerController>().interaction = gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            E.SetActive(false);
            collision.gameObject.GetComponent<PlayerController>().interaction = null;
        }
    }
}
