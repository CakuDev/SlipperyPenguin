using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuseEnemyBehaviour : ItemBehaviour
{
    public float refuseForce = 200;
    public float range = 7;
    CapsuleCollider playerCollider;

    private void Awake()
    {
        playerCollider = GameObject.FindWithTag(Tags.PLAYER).GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY);
        foreach(GameObject enemy in enemies) {
            float distance = Vector3.Distance(playerCollider.transform.position, enemy.transform.position);
            if (distance < range)
            {
                float forceByDistance = 1 - (distance / range);
                Vector3 direction = (enemy.transform.position - playerCollider.transform.position).normalized;
                enemy.GetComponent<Rigidbody>().AddForce(forceByDistance * refuseForce * direction, ForceMode.Force);
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
