using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float speed = 90;
    public float launchForce = 400;
    public float launchAngle = 2;

    private Vector3 movementDirection;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();


        Vector3 focalPointPosition = GameObject.FindGameObjectWithTag(Tags.FOCAL_POINT).transform.position;
        movementDirection = (focalPointPosition.normalized - transform.position);
        movementDirection.y = 0;
        movementDirection.Normalize();
        RandomMovementRotation();
    }   

    void FixedUpdate()
    {
        rb.AddForce(speed * movementDirection);
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
        //source.PlayOneShot(outOfWaterClip);
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
