using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int dir = 99;
    public float speed = 1;
    public int damage;

    private void Update()
    {
        if (dir == 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (speed * Time.deltaTime));
        }
        if (dir == 1)
        {
            transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime), transform.position.y);
        }
        if (dir == 2)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (speed * Time.deltaTime));
        }
        if (dir == 3)
        {
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y);
        }
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyController>().Damage(damage, dir);
            Destroy(gameObject);
        }
    }
}
