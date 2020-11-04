using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Stats stats;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    AttackCollider col;
    Inventory inv;
    Animator anim;
    public GameObject interaction;
    public float move_x, move_y;
    public GameObject collider_obj;
    public float attack_time = 0.5f;
    int dir;
    float invTime = 0.5f;
    float speed;
    bool sprint, attack, hit = false;
    public bool dead = false;


    void Start()
    {
        stats = GetComponent<Stats>();
        anim = GetComponent<Animator>();
        col = collider_obj.GetComponent<AttackCollider>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        inv = GetComponent<Inventory>();
    }

    void Update()
    {
        move_x = Input.GetAxis("Horizontal"); move_y = Input.GetAxis("Vertical");
        if (!dead)
        {
            Move();
        }

        inv.UseItem(GetNumKey());

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Move()
    {
        if (!attack)
        {
            transform.position = new Vector3(transform.position.x + ((move_x * speed) * Time.deltaTime), transform.position.y + ((move_y * speed) * Time.deltaTime), transform.position.z);
            WalkAnim();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = stats.sprintSpeed;
        }
        else
        {
            speed = stats.walkSpeed;
        }

        if (Input.GetMouseButton(0) && !attack)
        {
            attack = true;
            StartCoroutine(Attack());
        }
    }

    void WalkAnim()
    {
        anim.SetFloat("Horizontal", move_x);
        anim.SetFloat("Vertical", move_y);
        anim.SetFloat("Speed", speed);
        if (move_x > 0.5f)
        {
            dir = 1;
            collider_obj.transform.localPosition = new Vector3(0.3f, 0, 0);
        }
        else if (move_x < -0.5f)
        {
            dir = 3;
            collider_obj.transform.localPosition = new Vector3(-0.3f, 0, 0);
        }
        else if (move_y > 0.5f)
        {
            dir = 0;
            collider_obj.transform.localPosition = new Vector3(0, 0.3f, 0);
        }
        else
        {
            dir = 2;
            collider_obj.transform.localPosition = new Vector3(0, -0.3f, 0);
        }
    }
    
    IEnumerator Attack()
    {
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(attack_time);
        anim.SetBool("Attack", false);
        attack = false;
    }

    void Hit()
    {
        col.Hit(stats.hitStrength, dir);
    }

    void Interact()
    {
        if (interaction != null)
        {
            interaction.GetComponent<Interaction>().Interact();
        }
    }

    public void Damage(float damage, int dir, float knockback)
    {
        if (!dead)
        {
            Vector2 force = new Vector2(0, 0);
            if (dir == 0)
            {
                force = new Vector2(0, knockback);
            }
            if (dir == 1)
            {
                force = new Vector2(knockback, 0);
            }
            if (dir == 2)
            {
                force = new Vector2(0, -knockback);
            }
            if (dir == 3)
            {
                force = new Vector2(-knockback, 0);
            }
            StartCoroutine(HitRoutine(damage, force));
        }
    }

    IEnumerator HitRoutine(float damage, Vector2 force)
    {
        rb.AddForce(force);
        hit = true;
        stats.currentHealth -= damage;
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;

        if (stats.currentHealth <= 0)
        {
            dead = true;
            anim.SetBool("Dying", true);
            yield return new WaitForSeconds(0.1f);
            anim.SetBool("Dead", true);
            yield return new WaitForSeconds(2);
            //Destroy(this.gameObject);
        }

        yield return new WaitForSeconds(invTime);
        hit = false;
    }

    int GetNumKey()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            return 0;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            return 1;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            return 2;
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            return 3;
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            return 4;
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            return 5;
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            return 6;
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            return 7;
        }
        if (Input.GetKey(KeyCode.Alpha9))
        {
            return 8;
        }
        return 99;
    }

}
