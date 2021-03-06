﻿using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour
{
    public GameObject player;
    Inventory inventory;
    public List<Image> invSlots = new List<Image>();
    public Text gold;

    // Start is called before the first frame update
    void Start()
    {
        inventory = player.GetComponent<Inventory>();
    }

    private void Update()
    {
        gold.text = inventory.gold.ToString();
    }

    public void AddItem(int num, ITEM item)
    {
        invSlots[num].enabled = true;
        invSlots[num].sprite = item.sprite;
    }

    public void RemoveItem(int num)
    {
        invSlots[num].sprite = null;
        invSlots[num].enabled = false;
    }

}
