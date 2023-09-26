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
    public AudioSource audioSource;
    public AudioClip bubblesClip;
    public Joystick joystick;

    private Rigidbody playerRb;
    private Vector3 direction;
    private Vector3 lastRotationVector;
    [HideInInspector]
    public ItemBehaviour itemToUse;
    private bool isPlatformMobile;
    [HideInInspector]
    public ItemBoxBehaviour itemBoxOverposition;
    private bool isEnding = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityMultiplier;
        isPlatformMobile = PlatformUtils.IsPlatformMobile();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            CheckUseItem();
        }
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
        float horizontalInput = isPlatformMobile ? joystick.Direction.x : Input.GetAxis("Horizontal");
        playerRb.AddForce(horizontalInput * movementSpeed * Vector3.right);
        float verticalInput = isPlatformMobile ? joystick.Direction.y : Input.GetAxis("Vertical");
        playerRb.AddForce(verticalInput * movementSpeed * Vector3.forward);
        direction = new(horizontalInput, 0, verticalInput);
    }

    public void CheckUseItem()
    {
        UseItem();
        if (itemBoxOverposition != null) itemBoxOverposition.CollectItemBox(this);
    }

    void UseItem()
    {
        if(itemToUse != null)
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
        if (velocity.magnitude > maxVelocity)
        {
            velocity.Normalize();
            velocity *= maxVelocity;
            playerRb.velocity = velocity;
        }
    }

    public void RotatePlayer()
    {
        Vector3 rotationVector;
        if (direction.magnitude >= 0.3f)
        {
            rotationVector = Quaternion.FromToRotation(Vector3.back, direction.normalized).eulerAngles;
            lastRotationVector = rotationVector;
        }
        else
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

    public void EndGame()
    {
        if (!isEnding)
        {
            Time.timeScale = 0.5f;
            isEnding = true;
            audioSource.PlayOneShot(bubblesClip);
            Invoke(nameof(EndGameDelay), bubblesClip.length);
        }
    }

    private void EndGameDelay()
    {
        Time.timeScale = 1f;
        GameObject.Find("Level Manager").GetComponent<LevelController>().EndGame();
        Destroy(gameObject);
    }
}