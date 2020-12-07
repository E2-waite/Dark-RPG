using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
public class Item : MonoBehaviour
{
    public ITEMTYPE itemType;
    public ITEM item;
    public bool shop = false;
    public Sprite objSprite;
    public bool setup = false;
    void Start()
    {
        objSprite = GetComponent<SpriteRenderer>().sprite;
        if (itemType == ITEMTYPE.potion)
        {
            Potion potion = GetComponent<Potion>();

            item = new ITEM { type = ITEMTYPE.potion, potionType = potion.type, sprite = objSprite, value = potion.value };
        }
        setup = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !shop)
        {
            Inventory inv = collision.gameObject.GetComponent<Inventory>();
            if (inv.AddItem(item))
            {
                Destroy(gameObject);
            }
        }
    }
}
