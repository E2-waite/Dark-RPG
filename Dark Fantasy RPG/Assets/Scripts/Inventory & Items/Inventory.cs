using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
public class Inventory : MonoBehaviour
{
    public GameObject invObj;
    InventoryUI invUI;
    public float gold = 0;
    int maxInv = 5;
    ITEM[] slots;
    List<ITEM> inventory = new List<ITEM>();

    private void Start()
    {
        invUI = invObj.GetComponent<InventoryUI>();
        slots = new ITEM[maxInv];
    }

    public bool AddItem(ITEM item)
    {
        for (int i = 0; i < maxInv; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;
                invUI.AddItem(i, item);
                return true;
            }
        }
        return false;
    }


    public void UseItem(int num)
    {
        if (num < maxInv && slots[num] != null && ObjManager.Instance.Use(gameObject, slots[num]))
        {
            slots[num] = null;
            invUI.RemoveItem(num);
        }
    }

}
