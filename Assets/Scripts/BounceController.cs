using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceController : MonoBehaviour
{
    public float bounceForce = 100;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Rigidbody collidedRb)) { 
            Vector3 direction = (transform.position - collision.transform.position);
            direction.y = 0;
            direction.Normalize();
            ApplyBounceImpulse(direction, collidedRb);
            GetComponent<MovementController>().SetMovementDirection(direction);
            if (collision.gameObject.TryGetComponent(out MovementController movementController))
            {
                movementController.SetMovementDirection(-direction);
            }
        }
    }

    void ApplyBounceImpulse(Vector3 direction, Rigidbody collidedRb)
    {
        rb.velocity = Vector3.zero;
        collidedRb.velocity = Vector3.zero;
        rb.AddForce(bounceForce * direction, ForceMode.Impulse);
        collidedRb.AddForce(-bounceForce * direction, ForceMode.Impulse);
    }
}
