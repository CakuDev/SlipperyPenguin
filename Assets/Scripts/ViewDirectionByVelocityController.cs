using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewDirectionByVelocityController : MonoBehaviour
{
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Debug.Log(Quaternion.FromToRotation(Vector3.left, Vector3.back).eulerAngles);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = rb.velocity.normalized;
        velocity.y = 0;
        Vector3 rotation = Quaternion.FromToRotation(Vector3.left, velocity).eulerAngles;
        Debug.Log("Rotation: " + rotation);
        Debug.Log("Velocity: " + velocity.normalized);
        transform.rotation = Quaternion.Euler(rotation);
    }
}
