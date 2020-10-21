﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sprite;
    AttackCollider collider;
    Animator anim;
    public float move_x, move_y;
    public GameObject collider_obj;
    public float sprint_speed = 2;
    public float walk_speed = 1;
    public float hit_damage = 100;
    public float attack_time = 0.5f;
    public float startHealth = 100;
    public float currentHealth;
    int dir;
    float invTime = 0.5f;
    float speed;
    bool sprint, attack, hit = false;
    public bool dead = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        collider = collider_obj.GetComponent<AttackCollider>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = startHealth;
    }

    void Update()
    {
        move_x = Input.GetAxis("Horizontal"); move_y = Input.GetAxis("Vertical");
        if (!dead)
        {
            Move();
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
            speed = sprint_speed;
        }
        else
        {
            speed = walk_speed;
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
        collider.Hit(hit_damage, dir);
    }


    public void Damage(float damage, int dir)
    {
        if (!dead)
        {
            Vector2 force = new Vector2(0, 0);
            if (dir == 0)
            {
                force = new Vector2(0, 50);
            }
            if (dir == 1)
            {
                force = new Vector2(50, 0);
            }
            if (dir == 2)
            {
                force = new Vector2(0, -50);
            }
            if (dir == 3)
            {
                force = new Vector2(-50, 0);
            }
            StartCoroutine(HitRoutine(damage, force));
        }
    }

    IEnumerator HitRoutine(float damage, Vector2 force)
    {
        rb.AddForce(force);
        hit = true;
        currentHealth -= damage;
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;

        if (currentHealth <= 0)
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

}