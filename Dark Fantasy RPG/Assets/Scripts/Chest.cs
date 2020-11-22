using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    Animator anim;
    public GameObject coinPrefab;
    bool open = false;
    public int maxCoins = 5, minCoins = 3;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public bool Open()
    {
        anim.SetBool("Open", true);
        StartCoroutine(DropCoins(Random.Range(minCoins, maxCoins)));
        return true;
    }

    IEnumerator DropCoins(int num)
    {
        if (num == 0)
        {
            yield return new WaitForSeconds(0.5f);
        }

        for (int i = 0; i < num; i++)
        {
            GameObject gold = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }

        for (int i = 0; i < items.Count; i++)
        {
            GameObject item = Instantiate(items[i], transform.position, Quaternion.identity);
            item.GetComponent<LootArc>().arc = true;
        }
    }
}
