using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuseEnemyBehaviour : ItemBehaviour
{
    public float refuseForce = 200;
    public AudioSource audioSource;

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
            Vector3 direction = (enemy.position - playerTransform.position);
            direction.y = 0;
            direction.Normalize();
            enemy.GetComponent<Rigidbody>().AddForce(refuseForce * direction, ForceMode.Impulse);
        }
    }

    public override void OnActive()
    {
        base.OnActive();
        audioSource.Stop();
        audioSource.PlayOneShot(audioSource.clip);
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
