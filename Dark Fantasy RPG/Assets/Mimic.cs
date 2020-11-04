using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mimic : MonoBehaviour
{
    public GameObject chestPrefab;
    Animator anim;
    public bool open = false;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public bool Open()
    {
        StartCoroutine(OpenRoutine());
        return true;
    }

    IEnumerator OpenRoutine()
    {
        anim.SetBool("Open", true);
        yield return new WaitForSeconds(1);
        open = true;
    }

    public void Close()
    {
        open = false;
        anim.SetBool("Open", false);
    }

    public void DropChest()
    {
        StartCoroutine(ChestRoutine());
    }

    IEnumerator ChestRoutine()
    {
        open = false;
        anim.SetBool("Open", false);
        yield return new WaitForSeconds(1);
        Instantiate(chestPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
