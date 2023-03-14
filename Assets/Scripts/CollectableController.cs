using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    public int puntuationBonus = 10;
    public GameObject gameObjectToDestroy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.PLAYER))
        {
            GameObject.FindWithTag(Tags.GAME_CONTROLLER).GetComponent<LevelController>().UpdateScore(puntuationBonus);
            Destroy(gameObjectToDestroy);
        }
    }
}
