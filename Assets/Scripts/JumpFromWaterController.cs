using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFromWaterController : MonoBehaviour
{
    public float speed;
    public float launchForce;
    public float launchAngle;
    
    private Vector3 movementDirection;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        

        Vector3 focalPointPosition = GameObject.FindGameObjectWithTag(Tags.FOCAL_POINT).transform.position;
        movementDirection = (focalPointPosition - transform.position).normalized;
        RandomMovementRotation();

        Vector3 launchDirection = movementDirection;
        launchDirection.y = launchAngle;
        rb.AddForce(launchForce * launchDirection.normalized, ForceMode.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(speed * movementDirection);
    }

    void RandomMovementRotation()
    {
        movementDirection = Quaternion.AngleAxis(Random.Range(-30f, 30f), Vector3.up) * movementDirection;
    }

    public void SetMovementDirection(Vector3 direction)
    {
        movementDirection = direction;
    }
}
