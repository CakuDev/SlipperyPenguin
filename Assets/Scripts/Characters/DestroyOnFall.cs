using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnFall : MonoBehaviour
{
    public float limitHeight = -5;

    [HideInInspector]
    public ObjectPooling objectPooling;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= limitHeight)
        {
            if(transform.CompareTag(Tags.PLAYER))
            {
                GameObject.Find("Level Manager").GetComponent<LevelController>().EndGame();
                return;
            }
            objectPooling.SaveObject(gameObject);
        }
    }
}
