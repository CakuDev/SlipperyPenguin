using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float maxVelocity;
    public float gravityMultiplier;
    public Animator animator;
    public Image itemImage;

    private Rigidbody playerRb;
    private Vector3 direction;
    private Vector3 lastRotationVector;
    [HideInInspector]
    public ItemBehaviour itemToUse;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityMultiplier;
    }

    private void Update()
    {
        UseItem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        ManageMaxVelocity();
        RotatePlayer();
        ManageAnimation();
    }

    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(horizontalInput * movementSpeed * Vector3.right);
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(verticalInput * movementSpeed * Vector3.forward);
        direction = new(horizontalInput, 0, verticalInput);
    }

    void UseItem()
    {
        if(Input.GetButton("Fire1") && itemToUse != null)
        {
            itemToUse.OnActive();
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            itemToUse = null;
        }
    }

    void ManageMaxVelocity()
    {
        Vector3 velocity = playerRb.velocity;
        
        // Manage max velocity in x axis
        if(velocity.x >= maxVelocity)
        {
            velocity.x = maxVelocity;
        } else if (velocity.x <= -maxVelocity)
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

        playerRb.velocity = velocity;
    }

    public void RotatePlayer()
    {
        Vector3 rotationVector;
        if (direction.magnitude != 0)
        {
            rotationVector = Quaternion.FromToRotation(Vector3.back, direction.normalized).eulerAngles;
            lastRotationVector = rotationVector;
        } else
        {
            rotationVector = lastRotationVector;
        }
        rotationVector.x = 0;
        rotationVector.z = 0;
        transform.rotation = Quaternion.Euler(rotationVector);
    }

    public void ManageAnimation()
    {
        if(direction.sqrMagnitude >= 0.1f && animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            animator.SetTrigger("Walk");
            animator.ResetTrigger("Idle");
        } else if(direction.sqrMagnitude <= 0.1f && animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            animator.SetTrigger("Idle");
            animator.ResetTrigger("Walk");
        }
    }
}