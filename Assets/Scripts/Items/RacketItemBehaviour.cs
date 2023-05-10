using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketItemBehaviour : ItemBehaviour
{

    public PhysicMaterial racketMaterial;
    public float newSpeed;

    CapsuleCollider playerCollider;
    PlayerController playerController;
    PhysicMaterial previousMaterial;
    float previousSpeed;

    private void Awake()
    {
        playerCollider = GameObject.FindWithTag(Tags.PLAYER).GetComponent<CapsuleCollider>();
        playerController = playerCollider.GetComponent<PlayerController>();
    }

    public override void OnActive()
    {
        base.OnActive();
        previousMaterial = playerCollider.material;
        playerCollider.material = racketMaterial;
        previousSpeed = playerController.movementSpeed;
        playerController.movementSpeed = newSpeed;
    }

    public override void OnDesactivate()
    {
        playerCollider.material = previousMaterial;
        playerController.movementSpeed = previousSpeed;
        base.OnDesactivate();
    }
}
