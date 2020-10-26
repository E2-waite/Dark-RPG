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
        mana
    }
    
    public enum ITEMTYPE
    {
        NONE,
        potion,
        weapon,
        armour
    }

    public struct Item
    {
        public ITEMTYPE type;
        public Sprite sprite;
    }
}