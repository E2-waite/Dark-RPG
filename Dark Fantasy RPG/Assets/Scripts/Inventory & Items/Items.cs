using UnityEngine;

namespace Items
{
    enum HELMET
    {
        iron,
        gold,
        chain,
        hood
    }
    enum WEAPON
    {
        longsword,
        spear

    }
    public enum POTION
    {
        health,
        mana,
        speed
    }
    
    public enum ITEMTYPE
    {
        NONE,
        potion,
        weapon,
        armour
    }

    public class ITEM
    {
        public ITEMTYPE type;
        public POTION potionType;
        public Sprite sprite;
        public float value;
    }
}