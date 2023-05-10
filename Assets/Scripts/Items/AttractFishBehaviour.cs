using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractFishBehaviour : ItemBehaviour
{
    public float attractForce = 400;
    public float range = 7;
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
            float distance = Vector3.Distance(playerTransform.position, collectable.transform.position);
            if (distance < range)
            {
                float forceByDistance = 1 - (distance / range);
                Vector3 direction = (playerTransform.position - collectable.transform.position).normalized;
                collectable.GetComponent<Rigidbody>().AddForce(attractForce * forceByDistance * direction, ForceMode.Force);
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(other.CompareTag(Tags.COLLECTABLE))
        {
            collectables.Add(other.transform);
            Debug.Log(collectables.Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.COLLECTABLE))
        {
            collectables.Remove(other.transform);
            Debug.Log(collectables.Count);
        }
    }

    public override void OnActive()
    {
        collectables = new();
        base.OnActive();
    }

    public override void OnDesactivate()
    {
        base.OnDesactivate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
