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
    }

    public LOOT lootType;
    public float moveSpeed = 5;
    public int value = 1;
    public bool bounce = false;
    GameObject player;
    Stats stats;
    float dist;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<Stats>();
    }

    public void Init(GameObject p)
    {
        stats = p.GetComponent<Stats>();
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
            }
        }
    }

    bool NeedHealth()
    {
        if (stats.currentHealth < stats.maxHealth)
        {
            return true;
        }
        return false;
    }

    void AddHealth()
    {
        if (stats.currentHealth + value > stats.maxHealth)
        {
            stats.currentHealth = stats.maxHealth;
        }
        else
        {
            stats.currentHealth += value;
        }
    }
}
