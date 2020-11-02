using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatBar : MonoBehaviour
{
    public GameObject player;
    public Image healthBar, manaBar;
    public Text levelText;
    Stats stats;
    private void Start()
    {
        stats = player.GetComponent<Stats>();
    }

    private void Update()
    {
        float val = Mathf.Clamp(stats.currentHealth, 0, 100);
        healthBar.fillAmount = stats.currentHealth / stats.maxHealth;

        val = Mathf.Clamp(stats.currentMana, 0, 100);
        manaBar.fillAmount = stats.currentMana / stats.maxMana;

        levelText.text = stats.level.ToString();
    }
}
