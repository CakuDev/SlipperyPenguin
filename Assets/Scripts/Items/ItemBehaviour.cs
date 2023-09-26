using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBehaviour : MonoBehaviour
{
    public Sprite sprite;
    public float durationInSeconds = 5f;
    public float deactivationAlert = 2f;
    public GameObject objectToBlink;
    Coroutine currentCoroutine;

    public virtual void OnActive()
    {
        gameObject.SetActive(true);
        transform.parent.gameObject.SetActive(true);
        objectToBlink.SetActive(true);
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(Desactivate());
    }

    IEnumerator Desactivate()
    {
        StartCoroutine(DeactivationAlert());
        yield return new WaitForSeconds(durationInSeconds);
        OnDesactivate();
    }

    public virtual void OnDesactivate()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
        currentCoroutine = null;
        objectToBlink.SetActive(true);
    }

    IEnumerator DeactivationAlert()
    {
        yield return new WaitForSeconds(deactivationAlert);
        while (currentCoroutine != null)
        {
            
            objectToBlink.SetActive(false);
            yield return new WaitForSeconds(.1f);
            objectToBlink.SetActive(true);
            yield return new WaitForSeconds(.1f);
            objectToBlink.SetActive(false);
            yield return new WaitForSeconds(.1f);
            objectToBlink.SetActive(true);
            yield return new WaitForSeconds(.7f);
        }
    }
}
