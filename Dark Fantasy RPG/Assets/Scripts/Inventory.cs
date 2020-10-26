using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
public class Inventory : MonoBehaviour
{
    public GameObject invObj;
    InventoryUI invUI;
    public float gold = 0;
    public int maxInv = 5;
    List<Item> inventory = new List<Item>();

    private void Start()
    {
        invUI = invObj.GetComponent<InventoryUI>();
    }

    public bool AddItem(Item item)
    {
        if (inventory.Count < maxInv)
        {
            inventory.Add(item);
            invUI.AddItem(inventory.Count - 1, item);
            Debug.Log(inventory[inventory.Count - 1].type.ToString() + " in inventory slot " + inventory.Count.ToString());
            return true;
        }
        return false;
    }

}
