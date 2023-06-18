using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxBehaviour : MonoBehaviour
{
    public AudioClip pickUpSFX;
    public AudioClip notPickUpSFX;
    public Animator childAnimator;

    [HideInInspector]
    public ItemBehaviour itemBehaviour;
    [HideInInspector]
    public Image itemImage;
    [HideInInspector]
    public ObjectPooling itemBoxPooling;

    public void CollectItemBox(PlayerController playerController)
    {
        if (playerController.itemToUse != null)
        {
            playerController.audioSource.PlayOneShot(notPickUpSFX);
            childAnimator.SetTrigger("notPickUp");
            playerController.itemBoxOverposition = this;
            return;
        }
        itemImage.sprite = itemBehaviour.sprite;
        itemImage.color = Color.white;
        playerController.itemToUse = itemBehaviour;
        playerController.audioSource.PlayOneShot(pickUpSFX);
        itemBoxPooling.SaveObject(gameObject);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag(Tags.PLAYER))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController) CollectItemBox(playerController);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.PLAYER))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null) playerController.itemBoxOverposition = null;
        }
    }
}
