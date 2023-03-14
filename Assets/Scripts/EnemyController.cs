using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int scorePenalty = -5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(Tags.PLAYER))
        {
            GameObject.FindWithTag(Tags.GAME_CONTROLLER).GetComponent<LevelController>().UpdateScore(scorePenalty);
        }
    }
}
