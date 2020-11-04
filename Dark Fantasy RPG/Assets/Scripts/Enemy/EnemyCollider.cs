using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public GameObject player = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = null;
        }
    }

    public void Hit(float damage, int dir, float knockback)
    {
        if (player != null)
        {
            player.GetComponent<PlayerController>().Damage(damage, dir, knockback);
        }
    }
}
