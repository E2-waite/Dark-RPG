using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
public class Potion : MonoBehaviour
{
    public Sprite sprite;
    public POTION type;
    public float value;
    Item item;

    private void Start()
    {
        item = new Item { type = ITEMTYPE.potion, potionType = type, sprite = sprite, value = value };
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Inventory inv = collision.gameObject.GetComponent<Inventory>();
            if (inv.AddItem(item))
            {
                Destroy(gameObject);
            }
        }
    }
}
