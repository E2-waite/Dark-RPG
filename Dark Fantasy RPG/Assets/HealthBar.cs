using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public GameObject player;
    public Image healthBar;
    PlayerController pController;
    private void Start()
    {
        pController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        float val = Mathf.Clamp(pController.currentHealth, 0, 100);
        healthBar.fillAmount = pController.currentHealth / pController.startHealth;
    }
}
