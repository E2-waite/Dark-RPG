using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !enemies.Contains(collision.gameObject))
        {
            enemies.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && enemies.Contains(collision.gameObject))
        {
            enemies.Remove(collision.gameObject);
        }
    }

    public void Hit(float damage, int dir)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.Remove(enemies[i]);
            }
            else
            {
                enemies[i].GetComponent<EnemyController>().Damage(damage, dir);
            }
        }
    }

}
