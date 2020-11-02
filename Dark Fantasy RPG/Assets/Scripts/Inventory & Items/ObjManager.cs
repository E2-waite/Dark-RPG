using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using System.Linq.Expressions;

public class ObjManager : MonoSingleton<ObjManager>
{
    Potion potion;

    // Start is called before the first frame update
    void Start()
    {
        potion = GetComponent<Potion>();
    }

    public bool Use(GameObject player, Item item)
    {
        switch (item.type)
        {
            case ITEMTYPE.potion:
            {
                    return ConsumePotion(player, item);
                break;
            }
        }
        return false;
    }

    bool ConsumePotion(GameObject player, Item item)
    {
        Stats stats = player.GetComponent<Stats>();
        switch (item.potionType)
        {
            case POTION.health:
            {
                if (stats.currentHealth < stats.maxHealth)
                {
                    if (stats.currentHealth + item.value >= stats.maxHealth)
                    {
                        stats.currentHealth = stats.maxHealth;
                    }
                    else
                    {
                        stats.currentHealth += item.value;
                    }
                    return true; ;
                }
                return false;
            }
            case POTION.mana:
            {
                    if (stats.currentMana < stats.maxMana)
                    {
                        if (stats.currentMana + item.value >= stats.maxMana)
                        {
                            stats.currentMana = stats.maxMana;
                        }
                        else
                        {
                            stats.currentMana += item.value;
                        }
                        return true;
                    }
                    return false;
            }
        }
        return false;
    }
}
