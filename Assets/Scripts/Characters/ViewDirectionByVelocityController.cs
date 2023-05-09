using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewDirectionByVelocityController : MonoBehaviour
{
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = rb.velocity.normalized;
        velocity.y = 0;
        Vector3 rotation = Quaternion.FromToRotation(Vector3.left, velocity).eulerAngles;
        transform.rotation = Quaternion.Euler(rotation);
    }
}
