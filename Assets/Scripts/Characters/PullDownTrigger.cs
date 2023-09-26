using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullDownTrigger : MonoBehaviour
{
    public Rigidbody rb;
    public float force;
    public float radius;
    public float maxDistance;
    public float maxDistanceToWall;

    private void FixedUpdate()
    {
        //if (!Physics.Raycast(transform.position, Vector3.down, out _, LayerMask.GetMask("Ground")))
        //{
        //    Debug.Log("PULLING DOWN!");
        //    rb.AddForce(Vector3.down * force, ForceMode.Force);
        //}

        if(!Physics.SphereCast(transform.position, radius, Vector3.down, out RaycastHit hitInfo, maxDistance, LayerMask.GetMask("Ground")))
        {
            if (IsHoldingOnWall()) rb.AddForce(Vector3.up * force, ForceMode.Force);
            else rb.AddForce(Vector3.down * force, ForceMode.Force);
        }
    }

    public bool IsHoldingOnWall()
    {
        Vector3 spherePosition = transform.position + Vector3.down * maxDistance;
        return Physics.Raycast(spherePosition, Vector3.left, out _, maxDistanceToWall, LayerMask.GetMask("Ground"))
                || Physics.Raycast(spherePosition, Vector3.right, out _, maxDistanceToWall, LayerMask.GetMask("Ground"))
                || Physics.Raycast(spherePosition, Vector3.forward, out _, maxDistanceToWall, LayerMask.GetMask("Ground"))
                || Physics.Raycast(spherePosition, Vector3.back, out _, maxDistanceToWall, LayerMask.GetMask("Ground"));
    }

    private void OnDrawGizmos()
    {
        Vector3 spherePosition = transform.position + Vector3.down * maxDistance;
        Gizmos.DrawWireSphere(spherePosition, radius);
        Gizmos.DrawLine(spherePosition, spherePosition + Vector3.left * maxDistanceToWall);
        Gizmos.DrawLine(spherePosition, spherePosition + Vector3.right * maxDistanceToWall);
        Gizmos.DrawLine(spherePosition, spherePosition + Vector3.forward * maxDistanceToWall);
        Gizmos.DrawLine(spherePosition, spherePosition + Vector3.back * maxDistanceToWall);
    }
}
