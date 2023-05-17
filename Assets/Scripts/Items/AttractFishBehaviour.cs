using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractFishBehaviour : ItemBehaviour
{
    public float attractForce = 400;
    public AudioSource audioSource;
    
    Transform playerTransform;
    List<Transform> collectables;

    private void Awake()
    {
        playerTransform = GameObject.FindWithTag(Tags.PLAYER).transform;
        collectables = new();
    }

    private void FixedUpdate()
    {
        foreach(Transform collectable in collectables) {
            Vector3 direction = (playerTransform.position - collectable.transform.position).normalized;
            collectable.GetComponent<Rigidbody>().AddForce(attractForce * direction, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if(parent.CompareTag(Tags.COLLECTABLE))
        {
            collectables.Add(parent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent.CompareTag(Tags.COLLECTABLE))
        {
            collectables.Remove(parent);
        }
    }

    public override void OnActive()
    {
        base.OnActive();
        collectables = new();
        audioSource.Stop();
        audioSource.PlayOneShot(audioSource.clip);
    }

    public override void OnDesactivate()
    {
        base.OnDesactivate();
    }
}
