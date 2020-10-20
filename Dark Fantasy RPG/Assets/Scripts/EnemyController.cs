using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject collider;
    Animator anim;
    public GameObject player;
    public float damage = 25;
    public float startHealth = 100;
    public float currentHealth;
    public float dist;
    public float attackCooldown = 1;
    public int dir;
    float invTime = 0.25f;
    bool hit = false, inLine = false;
    SpriteRenderer sprite;
    bool attacked = false, dead = false;
    
    void Start()
    {
        currentHealth = startHealth;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        dist = Vector3.Distance(transform.position, player.transform.position);

        SetDir();

        if (dist <= 0.8f && !attacked && !player.GetComponent<PlayerController>().dead)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    void SetDir()
    {
        Vector3 diff = new Vector3(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y, 0);
        
        if (diff.y < 0 && diff.y < diff.x && -diff.y > diff.x)
        {
            dir = 0;
        }
        if (diff.x < 0 && diff.x < diff.y && -diff.x > diff.y)
        {
            dir = 1;
        }
        if (diff.y > 0 && diff.y > diff.x && -diff.y < diff.x)
        {
            dir = 2;
        }
        if (diff.x > 0 && diff.x > diff.y && -diff.x < diff.y)
        {
            dir = 3;
        }
    }

    public void Damage(float damage, int dir)
    {
        Vector2 force = new Vector2(0,0);
        if (dir == 0)
        {
            force = new Vector2(0, 100);
        }
        if (dir == 1)
        {
            force = new Vector2(100, 0);
        }
        if (dir == 2)
        {
            force = new Vector2(0, -100);
        }
        if (dir == 3)
        {
            force = new Vector2(-100, 0);
        }
        StartCoroutine(HitRoutine(damage, force));
    }

    IEnumerator HitRoutine(float damage, Vector2 force)
    {
        rb.AddForce(force);
        hit = true;
        currentHealth -= damage;
        sprite.color = Color.red;
        if (currentHealth <= 0)
        {
            dead = true;
            yield return new WaitForSeconds(0.1f);
            Destroy(this.gameObject);

        }
        yield return new WaitForSeconds(invTime);
        sprite.color = Color.white;
        hit = false;
    }

    IEnumerator AttackRoutine()
    {
        if (dir == 0)
        {
            collider.transform.localPosition = new Vector3(0, 0.3f, 0);
        }
        else if (dir == 1)
        {
            collider.transform.localPosition = new Vector3(0.3f, 0, 0);
        }
        else if (dir ==2)
        {
            collider.transform.localPosition = new Vector3(0, -0.3f, 0);
        }
        else if (dir == 3)
        {
            collider.transform.localPosition = new Vector3(-0.3f, 0, 0);
        }
        attacked = true;
        anim.SetInteger("Direction", dir);
        anim.SetBool("Attack", true);
        yield return new WaitForEndOfFrame();
        anim.SetBool("Attack", false);
        yield return new WaitForSeconds(attackCooldown);
        attacked = false;
    }

    void Hit()
    {
        if (!dead)
        {
            collider.GetComponent<EnemyCollider>().Hit(damage, dir);
        }
    }

}
