using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuseEnemyBehaviour : ItemBehaviour
{
    public float refuseForce = 200;
    
    Transform playerTransform;
    List<Transform> enemies;

    private void Awake()
    {
        enemies = new();
        playerTransform = GameObject.FindWithTag(Tags.PLAYER).transform;
    }

    private void FixedUpdate()
    {
        foreach(Transform enemy in enemies) {
            Vector3 direction = (enemy.position - playerTransform.position).normalized;
            enemy.GetComponent<Rigidbody>().AddForce( refuseForce * direction, ForceMode.Force);
        }
    }

    public override void OnActive()
    {
        base.OnActive();
    }

    public override void OnDesactivate()
    {
        enemies = new();
        base.OnDesactivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.ENEMY))
        {
            enemies.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.COLLECTABLE))
        {
            enemies.Remove(other.transform);
        }
    }
}
