using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcebergBehaviour : MonoBehaviour
{
    public float reducePerLevel;
    public float reductionSpeed;
    public LevelController levelController;
    public float minScale;
    public AudioSource audioSource;
    
    int level = 1;
    float initialScale;
    float goalScale;
    float previousScale;
    float lerpCount;

    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale.x;
        goalScale = initialScale;
        lerpCount = 1;
        level = levelController.currentLevel;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (levelController.currentLevel > level)
        {
            level = levelController.currentLevel;
            float scale = initialScale - reducePerLevel * level;
            if (scale > minScale)
            {
                lerpCount = 0;
                goalScale = scale;
                previousScale = transform.localScale.x;
                audioSource.Play();
            }
        }

        if (lerpCount <= 1)
        {
            float newScale = Mathf.Lerp(previousScale, goalScale, lerpCount);
            transform.localScale = new(newScale, initialScale, newScale);
            lerpCount += reductionSpeed;
            if (lerpCount >= 1) transform.localScale = new(goalScale, initialScale, goalScale);
        }
    }

}
