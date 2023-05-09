using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractFishBehaviour : ItemBehaviour
{
    public float attractForce = 400;
    public float range = 7;
    CapsuleCollider playerCollider;

    private void Awake()
    {
        playerCollider = GameObject.FindWithTag(Tags.PLAYER).GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        GameObject[] collectables = GameObject.FindGameObjectsWithTag(Tags.COLLECTABLE);
        foreach(GameObject collectable in collectables) {
            float distance = Vector3.Distance(playerCollider.transform.position, collectable.transform.position);
            if (distance < range)
            {
                float forceByDistance = 1 - (distance / range);
                Vector3 direction = (playerCollider.transform.position - collectable.transform.position).normalized;
                collectable.GetComponent<Rigidbody>().AddForce(attractForce * forceByDistance * direction, ForceMode.Force);
            }
            
        }
    }

    public override void OnActive()
    {
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
