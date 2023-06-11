using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInTimeBehaviour : MonoBehaviour
{
    public float secondsToDestroy;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyThis());
    }

    IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(secondsToDestroy);
        Destroy(gameObject);
    }
}
