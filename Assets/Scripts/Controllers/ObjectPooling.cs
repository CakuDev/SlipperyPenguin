using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public GameObject objectToPool;
    public int amountToInstantiate;

    private void Awake()
    {
        for(int i = 0; i < amountToInstantiate; i++)
        {
            InstantiateObject(transform);
        }
    }

    GameObject InstantiateObject(Transform parent)
    {
        // Instantiate inactive object and save it under this object as parent
        GameObject instantiatedObject = Instantiate(objectToPool, transform.position, transform.rotation, parent);
        instantiatedObject.SetActive(false);
        return instantiatedObject;
    }


    public GameObject GetObject()
    {
        // Use a pooled object or instantiate a new one
        if(transform.childCount > 0)
        {
            // Activate object to pool and take out of parent hierarchy
            Transform child = transform.GetChild(0);
            child.gameObject.SetActive(true);
            child.SetParent(transform.parent);
            // Set ObjectPooling object to save it when destroy it
            DestroyOnFall destroyOnFall;
            if (child.gameObject.TryGetComponent(out destroyOnFall))
                destroyOnFall.objectPooling = this;
            return child.gameObject;
        }
        // If there is no object in the child hierarchy, instantiate a new one
        return InstantiateObject(transform.parent);
    }

    public void SaveObject(GameObject objectToSave)
    {
        // Reset object to pool 
        objectToSave.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 0));
        objectToSave.transform.SetParent(transform);
        objectToSave.SetActive(false);
    }
}
