using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketItemBehaviour : ItemBehaviour
{

    public PhysicMaterial racketMaterial;

    CapsuleCollider playerCollider;
    PhysicMaterial previousMaterial;

    private void Awake()
    {
        playerCollider = GameObject.FindWithTag(Tags.PLAYER).GetComponent<CapsuleCollider>();
    }

    public override void OnActive()
    {
        base.OnActive();
        previousMaterial = playerCollider.material;
        playerCollider.material = racketMaterial;
    }

    public override void OnDesactivate()
    {
        playerCollider.material = previousMaterial;
        base.OnDesactivate();
    }
}
