using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBehaviour : MonoBehaviour
{
    public Sprite sprite;
    public float durationInSeconds = 5f;
    public virtual void OnActive()
    {
        gameObject.SetActive(true);
        transform.parent.gameObject.SetActive(true);
        StartCoroutine(Desactivate());
    }

    IEnumerator Desactivate()
    {
        yield return new WaitForSeconds(durationInSeconds);
        OnDesactivate();
    }

    public virtual void OnDesactivate()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }


}
