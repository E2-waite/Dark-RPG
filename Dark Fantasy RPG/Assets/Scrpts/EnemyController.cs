using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
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
    bool attacked = false;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startHealth;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        dist = Vector3.Distance(transform.position, player.transform.position);

        SetDir();

        if (dist <= 0.8f && !attacked)
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

    public void Damage(float damage, float hitDelay)
    {
        StartCoroutine(HitRoutine(damage, hitDelay));
    }

    IEnumerator HitRoutine(float damage, float hitDelay)
    {
        yield return new WaitForSeconds(hitDelay);
        hit = true;
        currentHealth -= damage;
        sprite.color = Color.red;
        yield return new WaitForSeconds(invTime);
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
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
        else if (dir ==2)a
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
        collider.GetComponent<EnemyCollider>().Hit(damage);
    }

}
