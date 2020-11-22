using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        idle,
        walk,
        attack
    }

    public enum Equipment
    {
        unarmed,
        sword,
        bow
    }

    public State state = State.idle;
    public Equipment combatState = Equipment.unarmed;
    Stats stats;
    Rigidbody2D rb;
    SpriteRenderer sprite;
    AttackCollider col;
    Inventory inv;
    Animator[] anim = new Animator[2];
    public GameObject interaction;
    public float move_x, move_y;
    public GameObject collider_obj;
    public GameObject arrowPrefab;
    public float attack_time = 0.5f;
    public int dir;
    bool moving = false;
    float invTime = 0.5f, combatTime = 15, currCombatTime;
    float speed;
    bool sprint, attack, hit = false;
    public bool dead = false;
    bool bow = false, bowCharging = false;
    float bowCooldown = 0;

    void Start()
    {
        stats = GetComponent<Stats>();
        anim[0] = GetComponent<Animator>();
        anim[1] = transform.GetChild(0).gameObject.GetComponent<Animator>();
        col = collider_obj.GetComponent<AttackCollider>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        inv = GetComponent<Inventory>();
    }

    void Update()
    {
        move_x = Input.GetAxis("Horizontal"); move_y = Input.GetAxis("Vertical");
        if (!attack)
        {
            if (((move_x > 0 || move_x < 0) || (move_y > 0 || move_y < 0)))
            {
                state = State.walk;
            }
            else
            {
                state = State.idle;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            anim[i].SetFloat("Horizontal", move_x);
            anim[i].SetFloat("Vertical", move_y);
            anim[i].SetFloat("Speed", speed / 30);
        }
        WalkAnim();
        if (!dead && !bow)
        {
            Move();
        }

        inv.UseItem(GetNumKey());

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

        if (Input.GetMouseButton(1))
        {
            anim[0].SetBool("Bow", true);
            bow = true;
            if (Input.GetMouseButton(0) && bowCooldown <= 0)
            {
                anim[0].SetBool("Charge", true);
                bowCharging = true;
            }
            else
            {
                if (bowCharging && bowCooldown <= 0)
                {
                    bowCharging = false;
                    anim[0].SetBool("Fire", true);
                    anim[0].SetBool("Charge", false);
                }
            }
        }
        else
        {
            anim[0].SetBool("Bow", false);
            bow = false;
            anim[0].SetBool("Charge", false);
            bowCharging = false;
        }

        GetMouseDir();
        if (!attack)
        {
            SetAnimLayerWeight();
        }
    }

    void GetMouseDir()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        mousePos = mousePos - transform.position;

        float x = mousePos.x, y = mousePos.y;

        //Debug.Log(mousePos.x.ToString() + mousePos.y.ToString());

        if (x < 0)
        {
            x = -x;
        }
        if (y < 0)
        {
            y = -y;
        }
        bool horizontal = true;

        if (y > x)
        {
            horizontal = false;
        }

        if (horizontal)
        {
            if (mousePos.x < 0)
            {
                anim[0].SetInteger("Direction", 3);
            }
            if (mousePos.x > 0)
            {
                anim[0].SetInteger("Direction", 1);
            }
        }
        if (!horizontal)
        {
            if (mousePos.y < 0)
            {

                anim[0].SetInteger("Direction", 2);
            }
            if (mousePos.y > 0)
            {
                anim[0].SetInteger("Direction", 0);
            }
        }
    }

    void SetAnimLayerWeight()
    {
        if (combatState == Equipment.unarmed)
        {
            anim[0].SetLayerWeight(1, 1);
            anim[0].SetLayerWeight(2, 0);
            anim[0].SetLayerWeight(3, 0);
            anim[0].SetLayerWeight(4, 0);
            anim[0].SetLayerWeight(5, 0);
        }
        if (combatState == Equipment.sword)
        {
            anim[0].SetLayerWeight(1, 0);
            anim[0].SetLayerWeight(5, 0);
            if (state == State.idle)
            {
                anim[0].SetLayerWeight(2, 1);
                anim[0].SetLayerWeight(3, 0);
                anim[0].SetLayerWeight(4, 0);
            }
            if (state == State.walk)
            {
                anim[0].SetLayerWeight(2, 0);
                anim[0].SetLayerWeight(3, 1);
                anim[0].SetLayerWeight(4, 0);
            }
            if (state == State.attack)
            {
                anim[0].SetLayerWeight(2, 0);
                anim[0].SetLayerWeight(3, 0);
                anim[0].SetLayerWeight(4, 1);
            }
        }
    }

    void Move()
    {
            //transform.position = new Vector3(transform.position.x + ((move_x * speed) * Time.deltaTime), transform.position.y + ((move_y * speed) * Time.deltaTime), transform.position.z);
        rb.MovePosition(new Vector2(transform.position.x + (move_x * speed), transform.position.y + (move_y * speed)));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = stats.sprintSpeed;
        }
        else
        {
            speed = stats.walkSpeed;
        }

        if (Input.GetMouseButton(0) && !attack && !bow)
        {
            attack = true;
            StartCoroutine(Attack());
        }
    }

    void WalkAnim()
    {

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
        else if (move_y < -0.5f)
        {
            dir = 2;
            collider_obj.transform.localPosition = new Vector3(0, -0.3f, 0);
        }
    }
    
    IEnumerator Attack()
    {
        state = State.attack;
        combatState = Equipment.sword;
        anim[0].SetBool("Attack", true);
        anim[0].SetLayerWeight(1, 0);
        anim[0].SetLayerWeight(2, 0);
        anim[0].SetLayerWeight(3, 0);
        anim[0].SetLayerWeight(4, 4);
        anim[0].SetLayerWeight(5, 0);
        if (currCombatTime <= 0)
        {
            StartCoroutine(SwordTimer());
        }
        else
        {
            currCombatTime = combatTime;
        }
        yield return new WaitForSeconds(0.4f);
        anim[0].SetBool("Attack", false);
        attack = false;
    }

    IEnumerator SwordTimer()
    {
        currCombatTime = combatTime;
        while (currCombatTime > 0)
        {
            currCombatTime -= Time.deltaTime;
            yield return null;
        }
        currCombatTime = 0;

        combatState = Equipment.unarmed;
    }

    void Hit()
    {
        col.Hit(stats.hitStrength, dir);
    }

    void Fire()
    {
        bowCooldown = 1;
        Debug.Log("FIRE");
        anim[0].SetBool("Fire", false);
        StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        GameObject arrow = Instantiate(arrowPrefab, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.identity);
        arrow.GetComponent<Arrow>().dir = dir;
        while (bowCooldown > 0)
        {
            bowCooldown -= Time.deltaTime;
            yield return null;
        }
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
        Instantiate(ParticleManager.Instance.blood, transform);
        rb.AddForce(force);
        hit = true;
        stats.currentHealth -= damage;
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;

        if (stats.currentHealth <= 0)
        {
            dead = true;
            anim[0].SetBool("Dying", true);
            yield return new WaitForSeconds(0.1f);
            anim[0].SetBool("Dead", true);
            yield return new WaitForSeconds(2);
            //Destroy(this.gameObject);
            GameOver.Instance.Display();
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
