using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullDownTrigger : MonoBehaviour
{
    public Rigidbody rigidBody;

    private bool isOut;

    private void FixedUpdate()
    {
        if (isOut && rigidBody != null) rigidBody.AddForce(Vector3.down, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        isOut = false;
    }

    private void OnTriggerExit(Collider other)
    {
        isOut = true;
    }
}
