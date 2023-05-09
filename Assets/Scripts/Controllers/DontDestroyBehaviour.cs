using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyBehaviour : MonoBehaviour
{

    static Dictionary<string, GameObject> instances = new();

    // Start is called before the first frame update
    void Awake()
    {
        if(!instances.ContainsKey(gameObject.name))
        {
            instances.Add(gameObject.name, gameObject);
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(gameObject);
        }
    }

    public static void DestroyObject(GameObject gameObject)
    {
        instances.Remove(gameObject.name);
        Destroy(gameObject);
    }
}
