using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float speed = 90;
    public float launchForce = 400;
    public float launchAngle = 2;
    public float maxVelocity = 9;

    private Vector3 movementDirection;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(ChangeRbTriggerProperty());

        Vector3 focalPointPosition = GameObject.FindGameObjectWithTag(Tags.FOCAL_POINT).transform.position;
        movementDirection = (focalPointPosition.normalized - transform.position);
        movementDirection.y = 0;
        movementDirection.Normalize();
        RandomMovementRotation();
    }

    IEnumerator ChangeRbTriggerProperty()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider>().isTrigger = false;
    }

    void FixedUpdate()
    {
        rb.AddForce(speed * movementDirection);
        ManageMaxVelocity();
    }

    void ManageMaxVelocity()
    {
        Vector3 velocity = rb.velocity;

        // Manage max velocity in x axis
        if (velocity.x >= maxVelocity)
        {
            velocity.x = maxVelocity;
        }
        else if (velocity.x <= -maxVelocity)
        {
            velocity.x = -maxVelocity;
        }

        // Manage max velocity in z axis
        if (velocity.z >= maxVelocity)
        {
            velocity.z = maxVelocity;
        }
        else if (velocity.z <= -maxVelocity)
        {
            velocity.z = -maxVelocity;
        }

        rb.velocity = velocity;
    }

    void RandomMovementRotation()
    {
        movementDirection = Quaternion.AngleAxis(Random.Range(-30f, 30f), Vector3.up) * movementDirection;
    }

    public void JumpFromWater()
    {
        Vector3 launchDirection = movementDirection;
        launchDirection.y = launchAngle;
        rb.AddForce(launchForce * launchDirection.normalized, ForceMode.Impulse);
    }

    public void SetMovementDirection(Vector3 direction)
    {
        movementDirection = direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 to = transform.position + movementDirection*1.5f;
        Gizmos.DrawCube(to, Vector3.one/2);
    }
}
