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

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag(Tags.PLAYER))
        {
            if(collision.GetComponent<PlayerController>().itemToUse != null)
            {
                collision.GetComponent<PlayerController>().audioSource.PlayOneShot(notPickUpSFX);
                childAnimator.SetTrigger("notPickUp");
                return;
            }
            itemImage.sprite = itemBehaviour.sprite;
            itemImage.color = Color.white;
            collision.GetComponent<PlayerController>().itemToUse = itemBehaviour;
            collision.GetComponent<PlayerController>().audioSource.PlayOneShot(pickUpSFX);
            itemBoxPooling.SaveObject(gameObject);
        }
    }
}
