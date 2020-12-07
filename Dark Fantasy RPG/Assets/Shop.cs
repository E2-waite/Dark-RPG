using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject item;
    GameObject itemObj;
    public int cost;
    bool setup = false;
    public void Start()
    {
        itemObj = Instantiate(item, transform);
        itemObj.GetComponent<Item>().shop = true;
    }

    private void LateUpdate()
    {
        if (!setup && itemObj.GetComponent<Item>().setup)
        {
            setup = true;
            string val = itemObj.GetComponent<Item>().item.value.ToString();
            GetComponent<Interaction>().display.GetComponent<TextMesh>().text = val;
        }
    }

    public bool Buy(GameObject player)
    {
        Inventory inv = player.GetComponent<Inventory>();
        Item itemInst = itemObj.GetComponent<Item>();
        if (inv.gold >= cost && inv.AddItem(itemInst.item))
        {
            inv.gold -= itemInst.item.value;
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
