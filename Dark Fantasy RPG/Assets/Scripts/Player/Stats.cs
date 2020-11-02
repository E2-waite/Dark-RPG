using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int level = 1;
    public float maxHealth = 100, currentHealth;
    public float maxMana = 100, currentMana;
    public int maxExp = 100, currentExp = 0;
    public float hitStrength = 10;
    public float sprintSpeed = 2, walkSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }
}
