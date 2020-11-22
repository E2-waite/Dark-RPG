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
        if (type == INTERACTION.mimic)
        {
            if (GetComponent<Mimic>().Open())
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
        if (collision.gameObject.tag == "PlayerCol")
        {
            if (active)
            {
                E.SetActive(true);
                collision.transform.parent.gameObject.GetComponent<PlayerController>().interaction = gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerCol")
        {
            E.SetActive(false);
            collision.transform.parent.gameObject.GetComponent<PlayerController>().interaction = null;
        }
    }
}
