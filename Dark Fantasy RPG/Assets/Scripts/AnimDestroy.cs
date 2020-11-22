using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDestroy : MonoBehaviour
{
    public float delay = 2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyDelay());
    }
    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
