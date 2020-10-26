using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
public class Loot : MonoBehaviour
{
    public Sprite sprite;
    public enum LOOT
    {
        gold,
        health,
        item

    }

    public LOOT lootType;
    public Item thisItem;
    public ITEMTYPE itemType;
    public float moveSpeed = 5;
    public int value = 1;
    GameObject player;
    PlayerController controller;
    float dist;

    private void Start()
    {
        if (lootType == LOOT.item)
        {
            thisItem = new Item { type = itemType, sprite = sprite };
        }
    }

    public void Init(GameObject p)
    {
        player = p;
        controller = p.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            dist = Vector3.Distance(transform.position, player.transform.position);
            if (lootType == LOOT.gold || (lootType == LOOT.health && NeedHealth()))
            {
                if (dist < 1)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Inventory inv = collision.gameObject.GetComponent<Inventory>();
            switch (lootType)
            {
                case LOOT.gold:
                    {
                        inv.gold += 1;
                        Destroy(gameObject);
                        break;
                    }
                case LOOT.health:
                    {
                        if (NeedHealth())
                        {
                            AddHealth();
                            Destroy(gameObject);
                        }
                        break;
                    }
                case LOOT.item:
                    {
                        if (inv.AddItem(thisItem))
                        {
                            Destroy(gameObject);
                        }
                        break;
                    }
            }
        }
    }

    bool NeedHealth()
    {
        if (controller.currentHealth < controller.startHealth)
        {
            return true;
        }
        return false;
    }

    void AddHealth()
    {
        if (controller.currentHealth + value > controller.startHealth)
        {
            controller.currentHealth = controller.startHealth;
        }
        else
        {
            controller.currentHealth += value;
        }
    }
}
