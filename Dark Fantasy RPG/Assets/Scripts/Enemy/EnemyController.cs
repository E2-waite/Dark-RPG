using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum TYPE
    {
        melee,
        mimic
    }
    public TYPE type;
    public GameObject coinPrefab, hOrbPrefab;
    Rigidbody2D rb;
    public GameObject collider;
    Animator anim;
    public GameObject player;
    public float damage = 25;
    public float startHealth = 100;
    public float currentHealth;
    public float attackCooldown = 1;
    public float speed = 1;
    public float chaseDist = 128;
    public float knockback = 50;
    public int dir;
    float invTime = 0.25f;
    bool hit = false, inLine = false;
    SpriteRenderer sprite;
    bool attacked = false, dead = false;
    Mimic mimic;
    public float AIDist = 16f;
    GameObject[] AI;
    void Start()
    {
        player = RoomController.Instance.player;
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = startHealth;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        if (type == TYPE.mimic)
        {
            mimic = GetComponent<Mimic>();
        }
        AI = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void Update()
    {

        anim.SetInteger("Direction", dir);
        SetDir();

        if (GetDist(false) && !attacked && !dead &&!player.GetComponent<PlayerController>().dead)
        {
            StartCoroutine(AttackRoutine());
        }
        if (!attacked && !dead)
        {
            anim.SetBool("Walking", true);
            Chase();
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }

    bool GetDist(bool greater)
    {
        float xDist = transform.position.x - player.transform.position.x;
        float yDist = transform.position.y - player.transform.position.y;
        if (xDist < 0)
        {
            xDist = -xDist;
        }
        if (yDist < 0)
        {
            yDist = -yDist;
        }

        if ((greater && xDist >= 1 && yDist >= 2) || (!greater && xDist < 32 && yDist < 20))
        {
            return true;
        }
        return false;
    }

    void Chase()
    {
        if (player != null)
        {

            Vector3 direction = player.transform.position - transform.position;
            direction = new Vector3(direction.x, direction.y, 0);
            transform.Translate(direction * (speed * Time.deltaTime));

            foreach (GameObject go in AI)
            {
                if (go != null && go != gameObject)
                {
                    float distance = Vector3.Distance(go.transform.position, transform.position);
                    if (distance <= AIDist)
                    {
                        direction = transform.position - go.transform.position;
                        direction = new Vector3(direction.x, direction.y, 0);
                        transform.Translate(direction * (speed * Time.deltaTime));
                    }
                }
            }

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
        if (type != TYPE.mimic || mimic.open)
        {
            Vector2 force = new Vector2(0, 0);
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
    }

    IEnumerator HitRoutine(float damage, Vector2 force)
    {
        hit = true;
        currentHealth -= damage;
        sprite.color = Color.red;
        if (currentHealth <= 0)
        {
            StartCoroutine(DeathRoutine());
            yield break;

        }
        rb.AddForce(force * 32);
        yield return new WaitForSeconds(invTime);
        sprite.color = Color.white;
        hit = false;
    }

    IEnumerator DeathRoutine()
    {
        dead = true;
        if (type == TYPE.mimic)
        {
            mimic.DropChest();
            yield return new WaitForSeconds(invTime);
            sprite.color = Color.white;
        }
        else
        {
            int num = Random.Range(0, 10);
            anim.SetFloat("AnimSpeed", 0.0f);
            if (num == 0)
            {
                yield return new WaitForSeconds(0.5f);
            }

            for (int i = 0; i < num; i++)
            {
                GameObject gold = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                gold.transform.parent = transform.parent;
                yield return new WaitForSeconds(0.2f);
            }

            GameObject healthObj = Instantiate(hOrbPrefab, transform.position, Quaternion.identity);
            healthObj.transform.parent = transform.parent;
            Destroy(this.gameObject);
        }
    }

    IEnumerator AttackRoutine()
    {
        attacked = true;
        anim.SetBool("Attack", true);
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

        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Attack", false);
        yield return new WaitForSeconds(attackCooldown);
        attacked = false;
    }

    void Hit()
    {
        if (!dead)
        {
            collider.GetComponent<EnemyCollider>().Hit(damage, dir, knockback);
        }
    }
}
